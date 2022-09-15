using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Values;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class HolderValuesQueryHandler : IAuthIdentityHandler<HolderValuesQuery,IEnumerable<VoucherValueDto>>
    {
        VouchersDbContext dbContext;

        public HolderValuesQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(HolderValuesQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            var authDomainAccounts = await dbContext.DomainAccounts.Where(a => a.IdentityId == authIdentityId && a.Id == query.HolderId).ToListAsync();

            if (!authDomainAccounts.Any())
                return new List<VoucherValueDto>();

            var authDomainAccount = authDomainAccounts.First();

            var valuesQuery = dbContext.VoucherValues.Join(
                dbContext.UnitTypes,
                v => v.Id,
                u => u.Id,
                (v, u) => new { Value = v, UnitType = u }
            ).GroupJoin(
                dbContext.Images,
                o => o.Value.ImageId,
                i => i.Id,
                (o, imgs) => new { Value = o.Value, UnitType = o.UnitType, Images = imgs }
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new { result.Value, result.UnitType, Image = image }
            );
            if(query.Ticker is not null)
                valuesQuery = valuesQuery.Where(o => o.Value.Ticker.Contains(query.Ticker));

            return await valuesQuery
                .Where(o =>
                   dbContext.Units.Where(
                        unit => dbContext.AccountItems
                        .Include(acc => acc.Holder)
                        .Include(acc => acc.Unit)
                        .Where(acc => acc.HolderId == query.HolderId && acc.Balance > 0).Select(acc => acc.Unit.Id)
                        .Contains(unit.Id) && unit.ValidTo >= DateTime.Today && unit.UnitType.Id == o.Value.Id
                    ).Any()
                ).Select(o =>
                    new VoucherValueDto
                    {
                        Id = o.Value.Id,
                        Ticker = o.Value.Ticker,
                        Description = o.Value.Description,
                        ImageBase64 = o.Value.ImageId == null ? null : Convert.ToBase64String(o.Image.CroppedContent),
                        IssuerId = o.Value.IssuerIdentityId,
                        Supply = o.UnitType.Supply,
                    }
                ).ToListAsync();
        }
    }
}

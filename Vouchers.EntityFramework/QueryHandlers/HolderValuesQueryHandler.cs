using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class HolderValuesQueryHandler : IAuthIdentityHandler<HolderValuesQuery,IEnumerable<VoucherValueDto>>
    {
        VouchersDbContext _dbContext;

        public HolderValuesQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(HolderValuesQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            var authDomainAccounts = await _dbContext.DomainAccounts.Where(a => a.IdentityId == authIdentityId && a.Id == query.HolderId).ToListAsync();

            if (!authDomainAccounts.Any())
                return new List<VoucherValueDto>();

            var authDomainAccount = authDomainAccounts.First();

            var valuesQuery = _dbContext.VoucherValues.Join(
                _dbContext.UnitTypes,
                v => v.Id,
                u => u.Id,
                (v, u) => new { Value = v, UnitType = u }
            ).Join(
                _dbContext.Identities,
                o => o.Value.IssuerIdentityId,
                i => i.Id,
                (o, i) => new { o.Value, o.UnitType, Identity = i }
            );

            if(query.Ticker is not null)
                valuesQuery = valuesQuery.Where(o => o.Value.Ticker.Contains(query.Ticker));

            if (query.IssuerName is not null)
                valuesQuery = valuesQuery.Where(o => (o.Identity.FirstName + o.Identity.LastName).Contains(query.IssuerName));

            //var accountItemsQuery = _dbContext.Units.Where(
            //    unit => _dbContext.AccountItems
            //    .Include(acc => acc.Holder)
            //    .Include(acc => acc.Unit)
            //    .Where(acc => acc.HolderId == query.HolderId && acc.Balance > 0).Select(acc => acc.Unit.Id)
            //    .Contains(unit.Id) && unit.ValidTo >= DateTime.Today 
            //);

            var accountItemsQuery = _dbContext.AccountItems
                .Include(acc => acc.HolderAccount)
                .Include(acc => acc.Unit)
                .Where(acc => acc.HolderAccountId == query.HolderId && acc.Balance > 0 && acc.Unit.ValidTo >= DateTime.Today);

            return await valuesQuery.Where(
                o => accountItemsQuery.Where(acc => acc.Unit.UnitType.Id == o.UnitType.Id).Any()
            ).Select(o =>
                new VoucherValueDto
                {
                    Id = o.Value.Id,
                    Ticker = o.Value.Ticker,
                    Description = o.Value.Description,
                    ImageId = o.Value.ImageId,
                    IssuerAccountId = o.UnitType.IssuerAccountId,
                    IssuerName = o.Identity.FirstName + " " + o.Identity.LastName,
                    IssuerEmail = o.Identity.Email,
                    Balance = accountItemsQuery.Where(acc => acc.Unit.UnitType.Id == o.UnitType.Id).Sum(item => item.Balance),
                    Supply = o.UnitType.Supply,
                }
            ).ToListAsync(cancellation);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class IssuerValuesQueryHandler : IAuthIdentityHandler<IssuerValuesQuery,IEnumerable<VoucherValueDto>>
    {
        VouchersDbContext dbContext;

        public IssuerValuesQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }     

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(IssuerValuesQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await dbContext.DomainAccounts.Include(a => a.Domain).FirstOrDefaultAsync(a => a.Id == query.IssuerDomainAccountId);
            if(issuerDomainAccount is null)
                return new List<VoucherValueDto>();

            var authDomainAccounts = await dbContext.DomainAccounts.Where(a => a.IdentityId == authIdentityId && a.Domain.Id == issuerDomainAccount.Domain.Id).ToListAsync();

            if(!authDomainAccounts.Any())
                return new List<VoucherValueDto>();

            var valuesQuery = dbContext.VoucherValues.AsQueryable()
                .Join(dbContext.UnitTypes, v => v.Id, u => u.Id, (v,u) => new { Value = v, UnitType = u })
                .GroupJoin(dbContext.Images, o => o.Value.ImageId, i => i.Id, (o, imgs) => new { Value = o.Value, UnitType = o.UnitType, Images = imgs }).SelectMany(
                    result => result.Images.DefaultIfEmpty(),
                    (result, image) => new {result.Value, result.UnitType, Image = image} 
                );
            if (query.Ticker is not null)
                valuesQuery = valuesQuery.Where(o => o.Value.Ticker.Contains(query.Ticker));

            return await valuesQuery
                .Where(o => o.Value.IssuerIdentityId == issuerDomainAccount.IdentityId)
                .Select(o =>
                    new VoucherValueDto
                    {
                        Id = o.Value.Id,
                        IssuerId = o.Value.IssuerIdentityId,
                        Supply = o.UnitType.Supply,
                        Ticker = o.Value.Ticker,
                        Description = o.Value.Description,
                        ImageBase64 = o.Image == null ? null : Convert.ToBase64String(o.Image.CroppedContent)
                    }
                ).ToListAsync();          
        }
    }
}

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
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Values;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class IssuerValuesQueryHandler : IHandler<IssuerValuesQuery,IEnumerable<VoucherValueDto>>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly VouchersDbContext _dbContext;

        public IssuerValuesQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
        {
            _authIdentityProvider = authIdentityProvider;
            _dbContext = dbContext;
        }     

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(IssuerValuesQuery query, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var issuerDomainAccount = await _dbContext.DomainAccounts.Include(a => a.Domain).FirstOrDefaultAsync(a => a.Id == query.IssuerAccountId);
            if(issuerDomainAccount is null)
                return new List<VoucherValueDto>();

            var authDomainAccounts = await _dbContext.DomainAccounts.Where(a => a.IdentityId == authIdentityId && a.Domain.Id == issuerDomainAccount.Domain.Id).ToListAsync();

            if(!authDomainAccounts.Any())
                return new List<VoucherValueDto>();

            var valuesQuery = _dbContext.VoucherValues.AsQueryable()
                .Join(_dbContext.UnitTypes, v => v.Id, u => u.Id, (v, u) => new { Value = v, UnitType = u });

            if (query.Ticker is not null)
                valuesQuery = valuesQuery.Where(o => o.Value.Ticker.Contains(query.Ticker));

            return await valuesQuery
                .Where(o => o.Value.IssuerIdentityId == issuerDomainAccount.IdentityId)
                .Select(o =>
                    new VoucherValueDto
                    {
                        Id = o.Value.Id,
                        IssuerAccountId = o.UnitType.IssuerAccountId,
                        Supply = o.UnitType.Supply,
                        Ticker = o.Value.Ticker,
                        Description = o.Value.Description,
                        ImageId = o.Value.ImageId,
                    }
                ).GetListPageQuery(query).ToListAsync();          
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Dtos;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Common.Application.Queries;
using Vouchers.Common.Application.Services;
using Vouchers.Common.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IssuerValuesQueryHandler : IRequestHandler<IssuerValuesQuery,IReadOnlyList<VoucherValueDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public IssuerValuesQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }     

    public async Task<Result<IReadOnlyList<VoucherValueDto>>> HandleAsync(IssuerValuesQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var issuerDomainAccount = await _dbContext.Set<DomainAccount>().Include(a => a.Domain).FirstOrDefaultAsync(a => a.Id == query.IssuerAccountId);
        if(issuerDomainAccount is null)
            return new List<VoucherValueDto>();

        var authDomainAccounts = await _dbContext.Set<DomainAccount>().Where(a => a.IdentityId == authIdentityId && a.Domain.Id == issuerDomainAccount.Domain.Id).ToListAsync();

        if(!authDomainAccounts.Any())
            return new List<VoucherValueDto>();

        var valuesQuery = _dbContext.Set<VoucherValue>().AsQueryable()
            .Join(_dbContext.Set<UnitType>(), v => v.Id, u => u.Id, (v, u) => new { Value = v, UnitType = u });

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
            ).GetListPageQuery(query).ToListAsync(cancellation);          
    }
}
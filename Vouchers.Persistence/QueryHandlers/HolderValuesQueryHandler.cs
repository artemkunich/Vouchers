using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Identities.Domain;
using Vouchers.Values.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class HolderValuesQueryHandler : IHandler<HolderValuesQuery,IReadOnlyList<VoucherValueDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public HolderValuesQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IReadOnlyList<VoucherValueDto>>>HandleAsync(HolderValuesQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(_cultureInfoProvider.GetCultureInfo());

        var authDomainAccounts = await _dbContext.Set<DomainAccount>().Where(a => a.IdentityId == authIdentityId && a.Id == query.HolderId).ToListAsync();

        if (!authDomainAccounts.Any())
            return new List<VoucherValueDto>();

        var authDomainAccount = authDomainAccounts.First();

        var valuesQuery = _dbContext.Set<VoucherValue>().Join(
            _dbContext.Set<UnitType>(),
            v => v.Id,
            u => u.Id,
            (v, u) => new { Value = v, UnitType = u }
        ).Join(
            _dbContext.Set<Identity>(),
            o => o.Value.IssuerIdentityId,
            i => i.Id,
            (o, i) => new { o.Value, o.UnitType, Identity = i }
        );

        if(query.Ticker is not null)
            valuesQuery = valuesQuery.Where(o => o.Value.Ticker.Contains(query.Ticker));

        if (query.IssuerName is not null)
            valuesQuery = valuesQuery.Where(o => (o.Identity.FirstName + o.Identity.LastName).Contains(query.IssuerName));

        var accountItemsQuery = _dbContext.Set<AccountItem>()
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
        ).GetListPageQuery(query).ToListAsync(cancellation);
    }
}
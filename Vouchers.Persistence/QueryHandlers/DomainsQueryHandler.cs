using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class DomainsQueryHandler : IHandler<DomainsQuery, IReadOnlyList<DomainDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public DomainsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IReadOnlyList<DomainDto>>> HandleAsync(DomainsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(_cultureInfoProvider.GetCultureInfo());

        var domainsQuery = _dbContext.Set<Domain>()
            .Include( domain => domain.Contract)
            .Select( domain =>
                new DomainDto {
                    Id = domain.Id,
                    Name = domain.Contract.DomainName,
                    Description = domain.Description,
                    IsPublic = domain.IsPublic,
                    MembersCount = domain.MembersCount,
                    ImageId = domain.ImageId
                }
            );

        if(!string.IsNullOrEmpty(query.Name))
            domainsQuery = domainsQuery.Where(domain => domain.Name.Contains(query.Name));

        domainsQuery = domainsQuery.GroupJoin(
            _dbContext.Set<DomainAccount>().Where(domain => domain.IdentityId == authIdentityId),
            domain => domain.Id,
            account => account.Domain.Id,
            (domain, accounts) => new { Domain = domain, Accounts = accounts }
        ).SelectMany(
            result => result.Accounts.DefaultIfEmpty(),
            (result, account) => new { result.Domain, account }
        ).Where(domain => domain.account == null).Select(domain => domain.Domain).GetListPageQuery(query);
            
        return await domainsQuery.ToListAsync(cancellation);
    }
}
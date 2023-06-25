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

internal sealed class DomainsQueryHandler : IRequestHandler<DomainsQuery, IReadOnlyList<DomainDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public DomainsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<DomainDto>>> HandleAsync(DomainsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

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
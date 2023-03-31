using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Common.Application.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Common.Application.Dtos;
using Vouchers.Common.Application.Queries;
using Vouchers.Common.Application.UseCases;
using System.Threading;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Services;
using Vouchers.Domains.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class DomainAccountsQueryHandler : IRequestHandler<DomainAccountsQuery, IReadOnlyList<DomainAccountDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public DomainAccountsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<DomainAccountDto>>> HandleAsync(DomainAccountsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        return await GetQuery(query, authIdentityId).ToListAsync(cancellation);
    }    

    private IQueryable<DomainAccountDto> GetQuery(DomainAccountsQuery query, Guid authIdentityId) {

        var authIdentityDomainAccountQuery = _dbContext.Set<DomainAccount>()
            .Include(a => a.Domain)
            .Where(a => a.Domain.Id == query.DomainId && a.IdentityId == authIdentityId);

        var domainAccountsQuery = _dbContext.Set<DomainAccount>()
            .Include(a => a.Domain).ThenInclude(d => d.Contract)
            .Where(a => a.Domain.Id == query.DomainId && a.IdentityId != authIdentityId);

        if (query.IncludeConfirmed && !query.IncludeNotConfirmed)
            domainAccountsQuery = domainAccountsQuery.Where(a => a.IsConfirmed);
        if (!query.IncludeConfirmed && query.IncludeNotConfirmed)
            domainAccountsQuery = domainAccountsQuery.Where(a => !a.IsConfirmed);

        var resultQuery = domainAccountsQuery.Join(
            authIdentityDomainAccountQuery,
            a => a.Domain.Id,
            a => a.Domain.Id,
            (a, c) => new
            {
                a.Id,
                a.Domain.Contract.DomainName,
                a.DomainId,
                a.IdentityId,
                a.IsAdmin,
                a.IsIssuer,
                IsOwner = a.IdentityId == a.Domain.Contract.PartyId,
                a.IsConfirmed,
            }
        );

        var identitiesQuery = _dbContext.Set<Identity>().AsQueryable();

        if (query.Email is not null)
            identitiesQuery = identitiesQuery.Where(identity => identity.Email.Contains(query.Email));
            
        if (query.Name is not null)
            identitiesQuery = identitiesQuery.Where(identity => (identity.FirstName + " " + identity.LastName).Contains(query.Name));

        return resultQuery.Join(
            identitiesQuery,
            a => a.IdentityId,
            i => i.Id,
            (account, identity) =>
                new DomainAccountDto()
                {
                    Id = account.Id,
                    DomainId = account.DomainId,
                    DomainName = account.DomainName,
                    Email = identity.Email,
                    Name = identity.FirstName + " " + identity.LastName,
                    IsAdmin = account.IsAdmin,
                    IsIssuer = account.IsIssuer,
                    IsOwner = account.IsOwner,
                    IsConfirmed = account.IsConfirmed,
                    ImageId = identity.ImageId
                }
        ).GetListPageQuery(query);
    }
}
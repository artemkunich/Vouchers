using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;
using System.Linq;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities;
using Vouchers.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;
using Vouchers.Application.Services;
using Vouchers.Domains;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IdentityDomainAccountsQueryHandler : IHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public IdentityDomainAccountsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DomainAccountDto>> HandleAsync(IdentityDomainAccountsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        return await GetQuery(query, authIdentityId).ToListAsync();
    }
            
    private IQueryable<DomainAccountDto> GetQuery(IdentityDomainAccountsQuery query, Guid authIdentityId) 
    {
        var domainAccountsQuery = _dbContext.Set<DomainAccount>()
            .Include(a => a.Domain).ThenInclude(d => d.Contract)
            .Where(a => a.IdentityId == authIdentityId && a.IsConfirmed);

        if (query.DomainName is not null)
            domainAccountsQuery = domainAccountsQuery.Where(d => d.Domain.Contract.DomainName.Contains(query.DomainName));

        return domainAccountsQuery.Select(a =>
            new DomainAccountDto
            {
                Id = a.Id,
                DomainId = a.Domain.Id,
                DomainName = a.Domain.Contract.DomainName,
                IsAdmin = a.IsAdmin,
                IsIssuer = a.IsIssuer,
                IsOwner = a.IdentityId == a.Domain.Contract.PartyId
            }
        ).GetListPageQuery(query);
    }
}
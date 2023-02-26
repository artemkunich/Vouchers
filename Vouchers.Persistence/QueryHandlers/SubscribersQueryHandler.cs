using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;
using System.Linq;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Services;
using Vouchers.Domains.Domain;
using Vouchers.Identities.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class SubscribersQueryHandler : IHandler<SubscribersQuery, IReadOnlyList<SubscriberDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public SubscribersQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {     
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<SubscriberDto>>> HandleAsync(SubscribersQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        return await GetQuery(query, authIdentityId).ToListAsync(cancellation);
    }
            

    private IQueryable<SubscriberDto> GetQuery(SubscribersQuery query, Guid authIdentityId)
    {
        var subscriptionQuery = _dbContext.Set<DomainAccount>()
            .Include(a => a.Domain)
            .Where(a => a.IdentityId == authIdentityId && a.Domain.Id == query.DomainId);

        var subscribersQuery = _dbContext.Set<DomainAccount>()
            .Include(a => a.Domain)
            .Where(a => a.Domain.Id == query.DomainId);

        subscribersQuery = subscribersQuery.Join(
            subscriptionQuery,
            subscriber => subscriber.Domain.Id,
            subscription => subscription.Domain.Id,
            (subscriber, subscription) =>
                subscriber
        );

        return subscribersQuery
            .Join(
                _dbContext.Set<Identity>(),
                subscription => subscription.IdentityId,
                identity => identity.Id,
                (domainAccount, identityDetail) => new SubscriberDto
                {
                    DomainAccountId = domainAccount.Id,
                    Email = identityDetail.Email,
                    FirstName = identityDetail.FirstName,
                    LastName = identityDetail.LastName
                }
            ).GetListPageQuery(query);
    }
}
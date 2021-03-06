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

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class SubscribersQueryHandler : IAuthIdentityHandler<SubscribersQuery, IEnumerable<SubscriberDto>>
    {
        VouchersDbContext dbContext;

        public SubscribersQueryHandler(VouchersDbContext dbContext)
        {           
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SubscriberDto>> HandleAsync(SubscribersQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IEnumerable<SubscriberDto> Handle(SubscribersQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();




        private IQueryable<SubscriberDto> GetQuery(SubscribersQuery query, Guid authIdentityId)
        {
            var subscriptionQuery = dbContext.DomainAccounts
                .Include(a => a.Identity)
                .Include(a => a.Domain)
                .Where(a => a.Identity.Id == authIdentityId && a.Domain.Id == query.DomainId);

            var subscribersQuery = dbContext.DomainAccounts
                .Include(a => a.Identity)
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
                    dbContext.IdentityDetails.Include(d => d.Identity),
                    subscription => subscription.Identity.Id,
                    identityDetail => identityDetail.Identity.Id,
                    (domainAccount, identityDetail) => new SubscriberDto
                    {
                        DomainAccountId = domainAccount.Id,
                        IdentityName = identityDetail.IdentityName,
                        FirstName = identityDetail.FirstName,
                        LastName = identityDetail.LastName
                    }
                );
        }
    }
}

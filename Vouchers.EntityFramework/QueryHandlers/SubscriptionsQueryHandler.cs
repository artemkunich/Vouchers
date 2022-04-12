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

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class SubscriptionsQueryHandler : IAuthIdentityHandler<SubscriptionsQuery, IEnumerable<SubscriptionDto>>
    {
        VouchersDbContext dbContext;

        public SubscriptionsQueryHandler(VouchersDbContext dbContext)
        {           
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SubscriptionDto>> HandleAsync(SubscriptionsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IEnumerable<SubscriptionDto> Handle(SubscriptionsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        private IQueryable<SubscriptionDto> GetQuery(SubscriptionsQuery query, Guid authIdentityId) {
            var domainAccountsQuery = dbContext.DomainAccounts
                .Include(a => a.Identity)
                .Include(a => a.Domain)
                .Where(a => a.Identity.Id == authIdentityId);

            var domainDetailsQuery = dbContext.DomainDetails
                .Include(d => d.Contract).ThenInclude(contract => contract.Domain).AsQueryable();

            if (query.DomainName is not null)
                domainDetailsQuery = domainDetailsQuery.Where(d => d.Contract.DomainName.Contains(query.DomainName));

            return domainAccountsQuery.Join(
                domainDetailsQuery,
                a => a.Domain.Id,
                d => d.Contract.Domain.Id,
                (a, d) => new SubscriptionDto
                {
                    DomainAccountId = a.Id,
                    DomainName = d.Contract.DomainName
                }
            );
        }
    }
}

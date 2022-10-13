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
using Vouchers.Domains;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class IdentityDomainOffersQueryHandler : IAuthIdentityHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>>
    {
        VouchersDbContext _dbContext;

        public IdentityDomainOffersQueryHandler(VouchersDbContext dbContext)
        {           
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainOfferDto>> HandleAsync(IdentityDomainOffersQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).Skip(query.PageIndex * query.PageSize).Take(query.PageSize).ToListAsync();

        IQueryable<DomainOfferDto> GetQuery(IdentityDomainOffersQuery query, Guid authIdentityId)
        {
            IQueryable<DomainOffer> domainOffersQuery = _dbContext.DomainOffers.Where(offer => offer.RecipientId == null || offer.RecipientId == authIdentityId);
            domainOffersQuery = domainOffersQuery.Where(offer => offer.ValidFrom <= DateTime.Now && offer.ValidTo > DateTime.Now);

            return domainOffersQuery.GroupJoin(
                _dbContext.DomainOffersPerIdentityCounters.Where(counter => counter.IdentityId == authIdentityId),
                offer => offer.Id, 
                counter => counter.OfferId,
                (offer, counters) => new { Offer = offer, Counters = counters }
            ).SelectMany(
                result => result.Counters.DefaultIfEmpty(),
                (result, counter) =>
                new DomainOfferDto
                {
                    Id = result.Offer.Id,
                    Name = result.Offer.Name,
                    Description = result.Offer.Description,
                    Amount = result.Offer.Amount.Amount,
                    Currency = result.Offer.Amount.Currency.ToString(),
                    MaxSubscribersCount = result.Offer.MaxSubscribersCount,
                    InvoicePeriod = result.Offer.InvoicePeriod.ToString(),
                    ValidFrom = result.Offer.ValidFrom,
                    ValidTo = result.Offer.ValidTo,
                    MaxContractsPerIdentity = result.Offer.MaxContractsPerIdentity,
                    ContractsPerIdentity = counter.Counter
                }
            ).Where(offer => offer.MaxContractsPerIdentity == null && offer.ContractsPerIdentity == null || offer.MaxContractsPerIdentity != offer.ContractsPerIdentity);
        }
    }
}

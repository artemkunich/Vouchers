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
using Vouchers.Application.Services;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IdentityDomainOffersQueryHandler : IHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public IdentityDomainOffersQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {           
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DomainOfferDto>> HandleAsync(IdentityDomainOffersQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        return await GetQuery(query, authIdentityId).ToListAsync();
    }
            

    IQueryable<DomainOfferDto> GetQuery(IdentityDomainOffersQuery query, Guid authIdentityId)
    {
        IQueryable<DomainOffer> domainOffersQuery = _dbContext.Set<DomainOffer>().Where(offer => offer.RecipientId == null || offer.RecipientId == authIdentityId);
        domainOffersQuery = domainOffersQuery.Where(offer => offer.ValidFrom <= DateTime.Now && offer.ValidTo > DateTime.Now);

        return domainOffersQuery.GroupJoin(
            _dbContext.Set<DomainOffersPerIdentityCounter>().Where(counter => counter.IdentityId == authIdentityId),
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
                    MaxMembersCount = result.Offer.MaxMembersCount,
                    InvoicePeriod = result.Offer.InvoicePeriod.ToString(),
                    ValidFrom = result.Offer.ValidFrom,
                    ValidTo = result.Offer.ValidTo,
                    MaxContractsPerIdentity = result.Offer.MaxContractsPerIdentity,
                    ContractsPerIdentity = counter.Counter
                }
        ).Where(
            offer => offer.MaxContractsPerIdentity == null && offer.ContractsPerIdentity == null || offer.MaxContractsPerIdentity != offer.ContractsPerIdentity
        ).GetListPageQuery(query);
    }
}
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;
using System.Linq;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities.Domain;
using Vouchers.Core.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Domains.Domain;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;
using Vouchers.Application;
using Vouchers.Application.Services;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class DomainOffersQueryHandler : IHandler<DomainOffersQuery, Result<IEnumerable<DomainOfferDto>>>
{
    private readonly VouchersDbContext _dbContext;

    public DomainOffersQueryHandler(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<DomainOfferDto>>> HandleAsync(DomainOffersQuery query, CancellationToken cancellation) =>
        await GetQuery(query).ToListAsync(cancellation);
    
    IQueryable<DomainOfferDto> GetQuery(DomainOffersQuery query)
    {
        IQueryable<DomainOffer> domainOffersQuery;

        if (query.RecipientId is null)
            domainOffersQuery = _dbContext.Set<DomainOffer>().Where(offer => offer.RecipientId == null);
        else
            domainOffersQuery = _dbContext.Set<DomainOffer>().Where(offer => offer.RecipientId == query.RecipientId);

        if (query.IncludePlanned != true)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.ValidFrom <= DateTime.Now);

        if (query.IncludeExpired != true)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.ValidTo > DateTime.Now);

        if (query.Name != null)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.Name.Contains(query.Name));

        if (query.Currency != null)
            domainOffersQuery = domainOffersQuery.Where(offer => (int)offer.Amount.Currency == query.Currency);

        if (query.InvoicePeriod != null)
            domainOffersQuery = domainOffersQuery.Where(offer => (int)offer.InvoicePeriod == query.InvoicePeriod);

        if(query.MinAmount != null)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.Amount.Amount >= query.MinAmount);

        if (query.MaxAmount != null)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.Amount.Amount <= query.MaxAmount);

        if (query.MinMaxSubscribersCount != null)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.MaxMembersCount >= query.MinMaxSubscribersCount);

        if (query.MaxMaxSubscribersCount != null)
            domainOffersQuery = domainOffersQuery.Where(offer => offer.MaxMembersCount <= query.MaxMaxSubscribersCount);

        return domainOffersQuery.Select(domainOffer =>
            new DomainOfferDto
            {
                Id = domainOffer.Id,
                Name = domainOffer.Name,
                Description = domainOffer.Description,
                Amount = domainOffer.Amount.Amount,
                Currency = domainOffer.Amount.Currency.ToString(),
                MaxMembersCount = domainOffer.MaxMembersCount,
                InvoicePeriod = domainOffer.InvoicePeriod.ToString(),
                ValidFrom = domainOffer.ValidFrom,
                ValidTo = domainOffer.ValidTo,
                MaxContractsPerIdentity = domainOffer.MaxContractsPerIdentity,
            }
        ).GetListPageQuery(query);
    }
}
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
    public class DomainOffersQueryHandler : IHandler<DomainOffersQuery,IPaginatedEnumerable<DomainOfferDto>>
    {
        VouchersDbContext dbContext;

        public DomainOffersQueryHandler(VouchersDbContext dbContext)
        {           
            this.dbContext = dbContext;
        }

        public async Task<IPaginatedEnumerable<DomainOfferDto>> HandleAsync(DomainOffersQuery query, CancellationToken cancellation) =>
            await PaginatedList<DomainOfferDto>.CreateAsync(GetQuery(query), query.PageIndex, query.PageSize);

        IQueryable<DomainOfferDto> GetQuery(DomainOffersQuery query)
        {
            var domainOffersQuery = dbContext.DomainOffers.Where(domain => domain.Recipient == null && domain.ValidFrom <= DateTime.Now && domain.ValidTo >= DateTime.Now);

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
                domainOffersQuery = domainOffersQuery.Where(offer => offer.MaxSubscribersCount >= query.MinMaxSubscribersCount);

            if (query.MaxMaxSubscribersCount != null)
                domainOffersQuery = domainOffersQuery.Where(offer => offer.MaxSubscribersCount <= query.MaxMaxSubscribersCount);

            return domainOffersQuery.Select(domainOffer =>
                new DomainOfferDto
                {
                    Id = domainOffer.Id,
                    Name = domainOffer.Name,
                    Description = domainOffer.Description,
                    Amount = domainOffer.Amount.Amount,
                    Currency = domainOffer.Amount.Currency.ToString(),
                    MaxSubscribersCount = domainOffer.MaxSubscribersCount,
                    InvoicePeriod = domainOffer.InvoicePeriod.ToString()
                }
            );
        }
    }
}

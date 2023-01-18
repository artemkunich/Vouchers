using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;

namespace Vouchers.Domains.Domain;

public sealed class DomainContract : AggregateRoot<Guid>
{
    public Guid OfferId { get; init; }
    public DomainOffer Offer { get; init; }

    public Guid OffersPerIdentityCounterId { get; init; }
    public DomainOffersPerIdentityCounter OffersPerIdentityCounter { get; init; }

    public Guid PartyId { get; init; }

    public DateTime CreatedDate { get; init; }

    public string DomainName { get; init; }

    public static DomainContract Create(Guid id, DomainOffer offer,
        DomainOffersPerIdentityCounter offersPerIdentityCounter, Guid partyId, string domainName,
        DateTime createdDate) => new()
    {
        Id = id,
        OfferId = offer.Id,
        Offer = offer,

        OffersPerIdentityCounterId = offersPerIdentityCounter.Id,
        OffersPerIdentityCounter = offersPerIdentityCounter,

        PartyId = partyId,
        DomainName = domainName,
        CreatedDate = createdDate,
    };
}


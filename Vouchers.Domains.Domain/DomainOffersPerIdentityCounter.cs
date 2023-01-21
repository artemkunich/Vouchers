using System;
using Vouchers.Domains.Domain.Properties;
using Vouchers.Primitives;

namespace Vouchers.Domains.Domain;

public sealed class DomainOffersPerIdentityCounter : Entity<Guid>
{
    public Guid OfferId { get; init; }
    public DomainOffer Offer { get; init; }

    public Guid IdentityId { get; init; }
    public int Counter { get; private set; }

    public static DomainOffersPerIdentityCounter Create(Guid id, DomainOffer offer, Guid identityId, int counter) => new()
    {
        Id = id,
        OfferId = offer.Id,
        Offer = offer,

        IdentityId = identityId,

        Counter = counter,
    };


    public void AddContract()
    {
        if (Counter + 1 > Offer.MaxContractsPerIdentity)
            throw new DomainsException(Resources.MaxContractsCountIsExceeded);

        Counter++;
    }
}


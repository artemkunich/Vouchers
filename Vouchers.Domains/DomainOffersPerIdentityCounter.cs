using System;
using Vouchers.Primitives;
using Vouchers.Domains.Properties;

namespace Vouchers.Domains;

public sealed class DomainOffersPerIdentityCounter : AggregateRoot<Guid>
{
    public Guid OfferId { get; private set; }
    public DomainOffer Offer { get; private set; }

    public Guid IdentityId { get; private set; }
    public int Counter { get; private set; }

    private DomainOffersPerIdentityCounter() { }
    private DomainOffersPerIdentityCounter(Guid id, DomainOffer offer, Guid identityId, int counter) : base(id)
    { 
        OfferId = offer.Id;
        Offer = offer;

        IdentityId = identityId;

        Counter = counter;
    }

    public static DomainOffersPerIdentityCounter Create(DomainOffer offer, Guid identityId) =>
        new DomainOffersPerIdentityCounter(Guid.NewGuid(), offer, identityId, 0);


    public void AddContract()
    {
        if (Counter + 1 > Offer.MaxContractsPerIdentity)
            throw new DomainsException(Resources.MaxContractsCountIsExceeded);

        Counter++;
    }
}


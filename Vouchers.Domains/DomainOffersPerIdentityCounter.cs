using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Entities;

namespace Vouchers.Domains
{
    public class DomainOffersPerIdentityCounter : Entity
    {
        public Guid OfferId { get; private set; }
        public DomainOffer Offer { get; private set; }

        public Guid IdentityId { get; private set; }
        public int Counter { get; private set; }

        private DomainOffersPerIdentityCounter() { }
        internal DomainOffersPerIdentityCounter(Guid id, DomainOffer offer, Guid identityId, int counter) : base(id)
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
                throw new DomainsException("Max count of contracts is exceeded");

            Counter++;
        }
    }
}

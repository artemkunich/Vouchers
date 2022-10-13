using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Domains
{
    public sealed class DomainContract : Entity
    {
        public Guid OfferId { get; }
        public DomainOffer Offer { get; }

        public Guid OffersPerIdentityCounterId { get; }
        public DomainOffersPerIdentityCounter OffersPerIdentityCounter { get; }

        public Guid PartyId { get; }

        public DateTime CreatedDate { get; }

        public string DomainName { get; }

        public static DomainContract Create(DomainOffer offer, DomainOffersPerIdentityCounter offersPerIdentityCounter, Guid partyId, string domainName) =>
            new DomainContract(Guid.NewGuid(), offer, offersPerIdentityCounter, partyId, domainName, DateTime.Now);

        internal DomainContract(Guid id, DomainOffer offer, DomainOffersPerIdentityCounter offersPerIdentityCounter, Guid partyId, string domainName, DateTime createdDate) : base(id)
        {
            OfferId = offer.Id;
            Offer = offer;
           
            OffersPerIdentityCounterId = offersPerIdentityCounter.Id;
            OffersPerIdentityCounter = offersPerIdentityCounter; 

            PartyId = partyId;
            DomainName = domainName;
            CreatedDate = createdDate;
        }
        private DomainContract()
        { }
    }
}

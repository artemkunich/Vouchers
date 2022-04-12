using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;

namespace Vouchers.Domains
{
    public class DomainContract
    {
        public Guid Id { get; }

        public string ContractNumber { get; }

        public DomainOffer Offer { get; set; }

        public Identity Party { get; }

        public DateTime CreatedDate { get; }

        public string DomainName { get; }

        public Domain Domain { get; set; }

        public static DomainContract CreateDomainContract(Domain domain, DomainOffer offer, Identity party, string name) =>
            new DomainContract(Guid.NewGuid(), offer, party, name, DateTime.Now);

        internal DomainContract(Guid id, DomainOffer offer, Identity party, string domainName, DateTime createdDate)
        {
            Id = id;
            Offer = offer;
            Party = party;
            DomainName = domainName;
            CreatedDate = createdDate;
        }
        private DomainContract()
        { }
    }
}

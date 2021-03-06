using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;

namespace Vouchers.Domains
{
    public class UpdateDomainContractRequest
    {
        public Guid Id { get; }

        public Domain Domain { get; }
        public DomainOffer Offer { get; }

        public DateTime CreatedDateTime { get; }

        public DateTime? _performedDateTime;
        public DateTime? PerformedDateTime
        {
            get => _performedDateTime;
            set
            {
                if (value < CreatedDateTime)
                    throw new DomainsException("Performed datetime < created datetime");

                _performedDateTime = value;
            }
        }

        public static UpdateDomainContractRequest CreateUpdateDomainContractRequest(Domain domain, DomainOffer offer) =>
           new UpdateDomainContractRequest(Guid.NewGuid(), domain, offer, DateTime.Now);

        public UpdateDomainContractRequest(Guid id, Domain domain, DomainOffer offer, DateTime createdDateTime)
        {
            Id = id;
            Domain = domain;
            Offer = offer;
            CreatedDateTime = createdDateTime;
        }
    }
}

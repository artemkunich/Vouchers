using System;
using Vouchers.Domains.Properties;
using Vouchers.Primitives;

namespace Vouchers.Domains;

public sealed class UpdateDomainContractRequest
{
    public Guid DomainId { get; }
    public Domain Domain { get; }

    public Guid OfferId { get; }
    public DomainOffer Offer { get; }

    public DateTime CreatedDateTime { get; }

    public DateTime? _performedDateTime;
    public DateTime? PerformedDateTime
    {
        get => _performedDateTime;
        set
        {
            if (value < CreatedDateTime)
                throw new DomainsException(Resources.PerformedDatetimeIsLessThanCreatedDatetime);

            _performedDateTime = value;
        }
    }

    public static UpdateDomainContractRequest CreateUpdateDomainContractRequest(Domain domain, DomainOffer offer) =>
       new UpdateDomainContractRequest(Guid.NewGuid(), domain, offer, DateTime.Now);

    private UpdateDomainContractRequest(Guid id, Domain domain, DomainOffer offer, DateTime createdDateTime)
    {
        DomainId = domain.Id;
        Domain = domain;

        OfferId = offer.Id;
        Offer = offer;
        
        CreatedDateTime = createdDateTime;
    }
}

using System;
using Vouchers.Primitives;

namespace Vouchers.Identities.DomainEvents;

public class IdentityUpdatedDomainEvent : DomainEvent
{
    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public string NewEmail { get; set; }

    public Guid? NewImageId { get; set; }

}
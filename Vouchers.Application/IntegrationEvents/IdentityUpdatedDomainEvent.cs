using System;
using Vouchers.Application.Abstractions;
using Vouchers.Primitives;

namespace Vouchers.Application.DomainEvents;

public class IdentityUpdatedIntegrationEvent : IntegrationEvent
{
    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public string NewEmail { get; set; }

    public Guid? NewImageId { get; set; }

}
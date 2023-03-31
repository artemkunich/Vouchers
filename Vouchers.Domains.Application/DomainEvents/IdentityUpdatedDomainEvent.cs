using System;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record IdentityUpdatedDomainEvent : IDomainEvent
{
    public string NewFirstName { get; set; }

    public string NewLastName { get; set; }

    public string NewEmail { get; set; }

    public Guid? NewImageId { get; set; }

}
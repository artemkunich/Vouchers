using System;
using Vouchers.Application.Abstractions;
using Vouchers.Application.DomainEvents;

namespace Vouchers.Application.EventMappers;

public class IdentityUpdatedEventMapper : IEventMapper<IdentityUpdatedDomainEvent,IdentityUpdatedIntegrationEvent>
{
    public IdentityUpdatedIntegrationEvent Map(IdentityUpdatedDomainEvent @event)
    {
        return new IdentityUpdatedIntegrationEvent()
        {
            Id = Guid.NewGuid(),
            NewEmail = @event.NewEmail,
            NewFirstName = @event.NewFirstName,
            NewLastName = @event.NewLastName,
            NewImageId = @event.NewImageId
        };
    }
}
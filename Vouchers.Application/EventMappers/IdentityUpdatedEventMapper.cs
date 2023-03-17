using System;
using Vouchers.Application.Abstractions;
using Vouchers.Application.DomainEvents;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Application.EventMappers;

public class IdentityUpdatedEventMapper : IEventMapper<IdentityUpdatedDomainEvent,IdentityUpdatedIntegrationEvent>
{
    private readonly IIdentifierProvider<Guid> _identifierProvider;

    public IdentityUpdatedEventMapper(IIdentifierProvider<Guid> identifierProvider)
    {
        _identifierProvider = identifierProvider;
    }

    public IdentityUpdatedIntegrationEvent Map(IdentityUpdatedDomainEvent @event)
    {
        return new IdentityUpdatedIntegrationEvent
        {
            Id = _identifierProvider.CreateNewId(),
            NewEmail = @event.NewEmail,
            NewFirstName = @event.NewFirstName,
            NewLastName = @event.NewLastName,
            NewImageId = @event.NewImageId
        };
    }
}
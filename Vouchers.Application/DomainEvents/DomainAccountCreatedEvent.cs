using System;
using Vouchers.Application.Abstractions;

namespace Vouchers.Application.DomainEvents;

public record DomainAccountCreatedEvent(Guid DomainAccountId, Guid IdentityId, Guid DomainId, DateTime CreatedDateTime, bool IsIssuer, bool IsAdmin, bool IsOwner, bool IsConfirmed) : IDomainEvent;
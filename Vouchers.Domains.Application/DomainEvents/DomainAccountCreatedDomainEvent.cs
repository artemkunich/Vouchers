using System;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record DomainAccountCreatedDomainEvent(Guid DomainAccountId, Guid IdentityId, Guid DomainId, DateTime CreatedDateTime, bool IsIssuer, bool IsAdmin, bool IsOwner, bool IsConfirmed) : IDomainEvent;
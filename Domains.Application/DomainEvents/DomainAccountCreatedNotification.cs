using System;
using Akunich.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record DomainAccountCreatedNotification(Guid DomainAccountId, Guid IdentityId, Guid DomainId, DateTime CreatedDateTime, bool IsIssuer, bool IsAdmin, bool IsOwner, bool IsConfirmed) : INotification;
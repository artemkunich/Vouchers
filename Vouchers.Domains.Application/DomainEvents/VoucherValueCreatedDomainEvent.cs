using System;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record VoucherValueCreatedDomainEvent(Guid VoucherValueId, Guid DomainAccountId, Guid DomainId, Guid IssuerIdentityId, string Ticker, string Description, Guid? ImageId) : IDomainEvent;
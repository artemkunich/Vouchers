using System;
using Akunich.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record VoucherValueCreatedNotification(Guid VoucherValueId, Guid DomainAccountId, Guid DomainId, Guid IssuerIdentityId, string Ticker, string Description) : INotification;
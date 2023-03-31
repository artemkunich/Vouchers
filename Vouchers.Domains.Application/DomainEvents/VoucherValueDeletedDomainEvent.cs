using System;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record VoucherValueDeletedDomainEvent(Guid VoucherValueId) : IDomainEvent;
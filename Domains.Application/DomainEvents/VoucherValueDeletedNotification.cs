using System;
using Akunich.Application.Abstractions;

namespace Vouchers.Domains.Application.DomainEvents;

public record VoucherValueDeletedNotification(Guid VoucherValueId) : INotification;
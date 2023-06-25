using System;
using Vouchers.Domains.Domain.Exceptions;
using Akunich.Domain.Abstractions;

namespace Vouchers.Domains.Domain;

public sealed class VoucherValue : AggregateRoot<Guid>
{
    public Guid DomainId { get; init; }
    public Guid IssuerIdentityId { get; init; }
    public string Ticker { get; set; }
    public string Description { get; set; }

    public static VoucherValue Create(Guid unitTypeId, Guid domainId, Guid issuerIdentityId, string ticker)
    {
        if (string.IsNullOrEmpty(ticker))
            throw new TickerIsNotSpecified();

        return new()
        {
            Id = unitTypeId,
            DomainId = domainId, //Is needed for unique constraint
            IssuerIdentityId = issuerIdentityId,
            Ticker = ticker
        };
    }
}

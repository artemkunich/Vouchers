using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Primitives;
using Vouchers.Values.Domain.Properties;

namespace Vouchers.Values.Domain;

public sealed class VoucherValue : AggregateRoot<Guid>
{
    public Guid DomainId { get; init; }
    public Guid IssuerIdentityId { get; init; }
    public string Ticker { get; set; }
    public string Description { get; set; }
    public Guid? ImageId { get; set; }

    public static VoucherValue Create(Guid unitTypeId, Guid domainId, Guid issuerIdentityId, string ticker)
    {
        if (string.IsNullOrEmpty(ticker))
            throw new VoucherValueException(Resources.TickerIsNotSpecified);

        return new()
        {
            Id = unitTypeId,
            DomainId = domainId, //Is needed for unique constraint
            IssuerIdentityId = issuerIdentityId,
            Ticker = ticker
        };
    }
}

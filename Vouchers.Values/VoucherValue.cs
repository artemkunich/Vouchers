using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Entities;
using Vouchers.Values.Properties;

namespace Vouchers.Values;

[AggregateRoot]
public sealed class VoucherValue : Entity<Guid>
{
    public Guid DomainId { get; }
    public Guid IssuerIdentityId { get; }
    public string Ticker { get; set; }
    public string Description { get; set; }
    public Guid? ImageId { get; set; }

    internal VoucherValue(Guid unitTypeId, Guid domainId, Guid issuerIdentityId, string ticker) : base(unitTypeId)
    {
        DomainId = domainId; //Is needed for unique constraint
        IssuerIdentityId = issuerIdentityId;

        if (string.IsNullOrEmpty(ticker))
            throw new VoucherValueException(Resources.TickerIsNotSpecified);
        Ticker = ticker;
    }

    private VoucherValue() { }

    public static VoucherValue Create(Guid unitTypeId, Guid domainId, Guid issuerIdentityId, string ticker)
    {
        return new VoucherValue(unitTypeId, domainId, issuerIdentityId, ticker);
    }
}

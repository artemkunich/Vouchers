using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Entities;

namespace Vouchers.Values
{
    public sealed class VoucherValue : Entity
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
                throw new VoucherValueException("Ticker must be specified");
            Ticker = ticker;
        }

        private VoucherValue() { }

        public static VoucherValue Create(Guid unitTypeId, Guid domainId, Guid issuerIdentityId, string ticker)
        {
            return new VoucherValue(unitTypeId, domainId, issuerIdentityId, ticker);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Core;

namespace Vouchers.Values
{
    public class VoucherValueDetail {

        public Guid Id { get; }

        public VoucherValue Value { get; }
        public Domain Domain { get; }

        private string _ticker;
        public string Ticker
        {
            get => _ticker;
            set
            {
                if (Value.Supply != 0)
                    throw new VoucherValueException("Cannot update ticker while supply is non-zero");
                _ticker = value;
            }
        }

        public string Description { get; set; }

        internal VoucherValueDetail(VoucherValue value, string ticker, string description)
        {
            Value = value;
            Domain = Value.Issuer.Domain; //Is needed for unique constraint

            if (string.IsNullOrEmpty(ticker))
                throw new VoucherValueException("Ticker must be specified");
            Ticker = ticker;
            Description = description;
        }

        internal VoucherValueDetail(Guid id, VoucherValue value, string ticker, string description) : this(value, ticker, description)
        {
            Id = id;
        }

        private VoucherValueDetail() { }
    }
}

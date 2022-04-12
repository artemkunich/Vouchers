using System;
using System.Collections.Generic;
using Vouchers.Core;

namespace Vouchers.Values
{
    public interface IVoucherValueDetailFactory
    {
        public VoucherValueDetail CreateVoucherValueDetail(VoucherValue value, string ticker, string description);
    }

    public class VoucherValueDetailFactory
    {
        public VoucherValueDetail CreateVoucherValueDetail(VoucherValue value, string ticker, string description)
        {
            return new VoucherValueDetail(value, ticker, description);
        }
    }
}

using System;

namespace Vouchers.Values
{
    public class VoucherValueException : Exception
    {
        internal VoucherValueException(string message) : base(message)
        {
        }
    }
}

using System;

namespace Vouchers.Values.Domain;

public class VoucherValueException : Exception
{
    internal VoucherValueException(string message) : base(message)
    {
    }
}

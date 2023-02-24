using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class NotPositiveAmountException : ArgumentOutOfRangeException
{
    public override string Message => "Amount is not positive";
}
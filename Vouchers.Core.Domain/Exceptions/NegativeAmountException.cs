using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class NegativeAmountException : ArgumentOutOfRangeException
{
    public override string Message => "Amount is negative";
}
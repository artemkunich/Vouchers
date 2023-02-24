using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class AmountIsGreaterThanBalanceException : InvalidOperationException
{
    public override string Message => "Amount is greater than balance";
}
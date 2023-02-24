using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class AmountIsGreaterThanSupplyException : InvalidOperationException
{
    public override string Message => "Amount is greater than supply";
}
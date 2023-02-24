using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class TransactionContainsExpiredUnitsException : InvalidOperationException
{
    public override string Message => "Transaction contains expired units";
}
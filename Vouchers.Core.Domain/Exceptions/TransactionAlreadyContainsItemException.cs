using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class TransactionAlreadyContainsItemException : InvalidOperationException
{
    public override string Message => "Transaction already contains item";
}
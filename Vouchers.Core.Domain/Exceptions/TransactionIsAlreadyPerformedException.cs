using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class TransactionIsAlreadyPerformedException : InvalidOperationException
{
    public override string Message => "Transaction is already performed";
}
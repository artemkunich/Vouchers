using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class TransactionRequestIsAlreadyPerformedException : InvalidOperationException
{
    public override string Message => "Transaction request is already performed";
}
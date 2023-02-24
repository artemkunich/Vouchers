using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class ItemDoesNotBelongToTransactionException : InvalidOperationException
{
    public override string Message => "Item does not belong to transaction";
}
using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestCreditorIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request creditor is not satisfied by transaction";
}
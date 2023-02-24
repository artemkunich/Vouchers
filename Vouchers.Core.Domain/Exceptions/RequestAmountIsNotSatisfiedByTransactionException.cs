using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestAmountIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request amount is not satisfied by transaction";
}
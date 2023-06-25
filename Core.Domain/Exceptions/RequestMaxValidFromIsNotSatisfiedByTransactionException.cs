using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestMaxValidFromIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request MaxValidFrom is not satisfied by transaction";
}
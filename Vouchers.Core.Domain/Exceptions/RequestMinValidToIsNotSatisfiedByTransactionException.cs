using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestMinValidToIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request MinValidTo is not satisfied by transaction";
}
using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestUnitIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request unit is not satisfied by transaction";
}
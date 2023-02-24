using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestDebtorIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request debtor is not satisfied by transaction";
}
using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class RequestMustBeExchangeableIsNotSatisfiedByTransactionException : InvalidOperationException
{
    public override string Message => "Request MustBeExchangeable is not satisfied by transaction";
}
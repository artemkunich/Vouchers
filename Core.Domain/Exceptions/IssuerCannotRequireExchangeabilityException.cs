using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class IssuerCannotRequireExchangeabilityException : InvalidOperationException
{
    public override string Message => "Issuer cannot require exchangeability";
}
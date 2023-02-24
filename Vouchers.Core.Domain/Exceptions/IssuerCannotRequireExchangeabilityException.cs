using System;

namespace Vouchers.Core.Domain.Exceptions;

public class IssuerCannotRequireExchangeabilityException : InvalidOperationException
{
    public override string Message => "Issuer cannot require exchangeability";
}
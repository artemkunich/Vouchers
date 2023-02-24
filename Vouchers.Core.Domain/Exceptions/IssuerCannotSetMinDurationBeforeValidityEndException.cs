using System;

namespace Vouchers.Core.Domain.Exceptions;

public class IssuerCannotSetMinDurationBeforeValidityEndException : InvalidOperationException
{
    public override string Message => "Issuer cannot set min duration before validity end";
}
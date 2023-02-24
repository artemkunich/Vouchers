using System;

namespace Vouchers.Core.Domain.Exceptions;

public class IssuerCannotSetMaxDurationBeforeValidityStartException : InvalidOperationException
{
    public override string Message => "Issuer cannot set max duration before validity start";
}
using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class IssuerCannotSetMaxDurationBeforeValidityStartException : InvalidOperationException
{
    public override string Message => "Issuer cannot set max duration before validity start";
}
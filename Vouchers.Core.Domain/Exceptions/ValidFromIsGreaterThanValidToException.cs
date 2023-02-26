using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class ValidFromIsGreaterThanValidToException : ArgumentException
{
    public override string Message => "ValidFrom is greater than ValidTo";
}
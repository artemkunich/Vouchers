using System;

namespace Vouchers.Core.Domain.Exceptions;

public class ValidFromIsGreaterThanValidToException : ArgumentException
{
    public override string Message => "ValidFrom is greater than ValidTo";
}
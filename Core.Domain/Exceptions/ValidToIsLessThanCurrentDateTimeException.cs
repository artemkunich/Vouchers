using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class ValidToIsLessThanCurrentDateTimeException : ArgumentException
{
    public override string Message => "ValidTo is less than current dateTime";
}
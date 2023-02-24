using System;

namespace Vouchers.Core.Domain.Exceptions;

public class ValidToIsLessThanCurrentDateTimeException : ArgumentException
{
    public override string Message => "ValidTo is less than current dateTime";
}
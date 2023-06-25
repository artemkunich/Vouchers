using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class UnitIsExpiredException : InvalidOperationException
{
    public override string Message => "Unit is expired";
}
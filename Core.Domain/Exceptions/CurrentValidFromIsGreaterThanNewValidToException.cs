using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class CurrentValidFromIsGreaterThanNewValidToException : InvalidOperationException
{
    public override string Message => "Current ValidFrom is greater than new ValidTo";
}
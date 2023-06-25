using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class NewValidFromIsGreaterThanCurrentValidToException : InvalidOperationException
{
    public override string Message => "New ValidFrom is greater than current ValidTo";
}
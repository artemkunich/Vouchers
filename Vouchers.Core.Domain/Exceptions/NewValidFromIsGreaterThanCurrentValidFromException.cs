using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class NewValidFromIsGreaterThanCurrentValidFromException : InvalidOperationException
{
    public override string Message => "New ValidFrom is greater than current ValidFrom";
}
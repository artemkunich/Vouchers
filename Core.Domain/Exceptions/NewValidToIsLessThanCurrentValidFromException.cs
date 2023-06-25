using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class NewValidToIsLessThanCurrentValidFromException : InvalidOperationException
{
    public override string Message => "New ValidTo is less than current ValidFrom";
}
using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class ItemUnitCannotBeExchangedException : InvalidOperationException
{
    public override string Message => "Item unit cannot be exchanged";
}
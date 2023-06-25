using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class DisableExchangeabilityException : InvalidOperationException
{
    public override string Message => "Cannot disable exchangeability";
}
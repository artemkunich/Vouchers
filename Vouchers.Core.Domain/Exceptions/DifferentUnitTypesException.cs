using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class DifferentUnitTypesException : InvalidOperationException
{
    public override string Message => "Cannot operate with different unit types";
}
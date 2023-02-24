using System;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain;

public sealed class UnitQuantity
{       
    public decimal Amount { get; init; }

    public Guid UnitId { get; init; }
    public Unit Unit { get; init; }

    public static UnitQuantity Create(decimal amount, Unit unit)
    {
        if(amount <= 0)
            throw new NotPositiveAmountException();
        
        return new()
        {
            Amount = amount,
            UnitId = unit.Id,
            Unit = unit
        };
    }
}
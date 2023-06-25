using System;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain;

public sealed class UnitTypeQuantity
{       
    public decimal Amount { get; init; }

    public Guid UnitTypeId { get; init; }
    public UnitType UnitType { get; init; }

    public static UnitTypeQuantity Create(decimal amount, UnitType unitType)
    {
        if(amount < 0)
            throw new NegativeAmountException();
        
        return new()
        {
            Amount = amount,
            UnitTypeId = unitType.Id,
            UnitType = unitType,
        };
    }
    
    public UnitTypeQuantity Add(UnitQuantity unitQuantity)
    {
        if (UnitType.NotEquals(unitQuantity.Unit.UnitType))
            throw new DifferentUnitTypesException();

        return Create(Amount + unitQuantity.Amount, UnitType);
    }
}
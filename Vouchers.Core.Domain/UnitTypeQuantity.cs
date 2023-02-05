using System;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class UnitTypeQuantity
{       
    public decimal Amount { get; init; }

    public Guid UnitTypeId { get; init; }
    public UnitType UnitType { get; init; }

    public static UnitTypeQuantity Create(decimal amount, UnitType unitType) => new()
    {
        Amount = amount,
        UnitTypeId = unitType.Id,
        UnitType = unitType,
    };
    
    public UnitTypeQuantity Add(UnitQuantity unitQuantity, CultureInfo cultureInfo = null)
    {
        if (UnitType.NotEquals(unitQuantity.Unit.UnitType))
            throw CoreException.CannotOperateWithDifferentUnitTypes;

        return Create(Amount + unitQuantity.Amount, UnitType);
    }
}
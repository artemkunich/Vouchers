using System;
using System.Globalization;
using Vouchers.Primitives;

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
    
    public Result<UnitTypeQuantity> Add(UnitQuantity unitQuantity, CultureInfo cultureInfo = null) =>
        Result.Create()
            .IfTrueAddError(UnitType.NotEquals(unitQuantity.Unit.UnitType),
                Errors.CannotOperateWithDifferentUnitTypes(cultureInfo))
            .SetValue(Create(Amount + unitQuantity.Amount, UnitType));
}
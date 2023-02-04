using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class UnitQuantity
{       
    public decimal Amount { get; init; }
    public Guid UnitId { get; init; }
    public Unit Unit { get; init; }

    public static Result<UnitQuantity> Create(decimal amount, Unit unit)
    {
        return new UnitQuantity()
        {
            Amount = amount,
            UnitId = unit.Id,
            Unit = unit
        };
    }
    
    private UnitQuantity()
    {
        //Empty
    }
}
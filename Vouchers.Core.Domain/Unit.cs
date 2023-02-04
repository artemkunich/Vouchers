using System;
using System.Globalization;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class Unit : AggregateRoot<Guid>
{
    public Guid UnitTypeId { get; init; }
    public UnitType UnitType { get; init; }
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidTo { get; private set; }
    public bool CanBeExchanged { get; private set; }
    public decimal Supply { get; private set; }

    public static Result<Unit> Create(Guid id, DateTime validFrom, DateTime validTo, bool canBeExchanged, UnitType unitType, decimal supply, CultureInfo cultureInfo = null) =>
        Result.Create()
            .IfTrueAddError(validTo < DateTime.Today, Errors.ValidToIsLessThanToday(cultureInfo))
            .IfTrueAddError(validFrom > validTo, Errors.ValidFromIsGreaterThanValidTo(cultureInfo))
            .SetValue(new Unit
                {
                    Id = id,
                    ValidFrom = validFrom,
                    ValidTo = validTo,
                    CanBeExchanged = canBeExchanged,
                    UnitTypeId = unitType.Id,
                    UnitType = unitType,
                    Supply = supply
                }
            );

    private Unit()
    {
        //Empty
    }
    
    public Result<Unit> IncreaseSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .Process(unit => unit.Supply += amount)
            .MergeResultErrors(unit => unit.UnitType.IncreaseSupply(amount));

    public Result<Unit> ReduceSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .IfTrueAddError(Supply < amount, Errors.AmountIsGreaterThanSupply(cultureInfo))
            .Process(unit => unit.Supply -= amount)
            .MergeResultErrors(unit => unit.UnitType.ReduceSupply(amount));

    public Result<Unit> SetValidFrom(DateTime validFrom, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(validFrom > ValidFrom && Supply != 0,
                Errors.NewValidFromIsGreaterThanCurrentValidFrom(cultureInfo))
            .IfTrueAddError(validFrom > ValidTo, Errors.NewValidFromIsGreaterThanCurrentValidTo(cultureInfo))
            .Process(unit => unit.ValidFrom = validFrom);

    public Result<Unit> SetValidTo(DateTime validTo, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(validTo < ValidTo && Supply != 0, Errors.NewValidToIsLessThanCurrentValidFrom(cultureInfo))
            .IfTrueAddError(ValidFrom > validTo, Errors.CurrentValidFromIsGreaterThanNewValidTo(cultureInfo))
            .Process(unit => unit.ValidTo = validTo);


    public Result<Unit> SetCanBeExchanged(bool canBeExchanged, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(!canBeExchanged && CanBeExchanged && Supply != 0,
                Errors.CannotDisableExchangeability(cultureInfo))
            .Process(unit => unit.CanBeExchanged = canBeExchanged);

    public bool CanBeRemoved() => Supply == 0;
}
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

    public static Unit Create(Guid id, DateTime validFrom, DateTime validTo, bool canBeExchanged, UnitType unitType, decimal supply, CultureInfo cultureInfo = null)
    {
        if (validTo < DateTime.Today)
            throw CoreException.ValidToIsLessThanToday;

        if (validFrom > validTo)
            throw CoreException.ValidFromIsGreaterThanValidTo;

        return new()
        {
            Id = id,
            ValidFrom = validFrom,
            ValidTo = validTo,
            CanBeExchanged = canBeExchanged,

            UnitTypeId = unitType.Id,
            UnitType = unitType,

            Supply = supply
        };
    }

    public void IncreaseSupply(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;
        Supply += amount;

        UnitType.IncreaseSupply(amount);
    }

    public void ReduceSupply(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;
        if (Supply < amount)
            throw CoreException.AmountIsGreaterThanSupply;
        Supply -= amount;

        UnitType.ReduceSupply(amount);
    }

    public void SetValidFrom(DateTime validFrom, CultureInfo cultureInfo = null)
    {
        if (validFrom > ValidFrom && Supply != 0)
            throw CoreException.NewValidFromIsGreaterThanCurrentValidFrom;

        if (validFrom > ValidTo)
            throw CoreException.NewValidFromIsGreaterThanCurrentValidTo;

        ValidFrom = validFrom;
    }

    public void SetValidTo(DateTime validTo, CultureInfo cultureInfo = null)
    {
        if (validTo < ValidTo && Supply != 0)
            throw CoreException.NewValidToIsLessThanCurrentValidFrom;

        if (ValidFrom > validTo)
            throw CoreException.CurrentValidFromIsGreaterThanNewValidTo;

        ValidTo = validTo;
    }

    public void SetCanBeExchanged(bool canBeExchanged, CultureInfo cultureInfo = null)
    {
        if (!canBeExchanged && CanBeExchanged && Supply != 0)
            throw CoreException.CannotDisableExchangeability;

        CanBeExchanged = canBeExchanged;
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}
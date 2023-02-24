using System;
using System.Globalization;
using Vouchers.Core.Domain.Exceptions;
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

    public static Unit Create(Guid id, DateTime validFrom, DateTime validTo, DateTime currentDateTime, bool canBeExchanged, UnitType unitType)
    {
        if (validTo <= currentDateTime)
            throw new ValidToIsLessThanCurrentDateTimeException();
        
        if (validFrom > validTo)
            throw new ValidFromIsGreaterThanValidToException();

        return new()
        {
            Id = id,
            ValidFrom = validFrom,
            ValidTo = validTo,
            CanBeExchanged = canBeExchanged,

            UnitTypeId = unitType.Id,
            UnitType = unitType
        };
    }

    public void IncreaseSupply(decimal amount)
    {
        if (amount <= 0)
            throw new NotPositiveAmountException();
        Supply += amount;

        UnitType.IncreaseSupply(amount);
    }

    public void ReduceSupply(decimal amount)
    {
        if (amount <= 0)
            throw new NotPositiveAmountException();
        if (Supply < amount)
            throw new AmountIsGreaterThanSupplyException();
        Supply -= amount;

        UnitType.ReduceSupply(amount);
    }

    public void SetValidFrom(DateTime validFrom)
    {
        if (validFrom > ValidFrom && Supply != 0)
            throw new NewValidFromIsGreaterThanCurrentValidFromException();

        if (validFrom > ValidTo)
            throw new NewValidFromIsGreaterThanCurrentValidToException();

        ValidFrom = validFrom;
    }

    public void SetValidTo(DateTime validTo)
    {
        if (validTo < ValidTo && Supply != 0)
            throw new NewValidToIsLessThanCurrentValidFromException();

        if (ValidFrom > validTo)
            throw new CurrentValidFromIsGreaterThanNewValidToException();

        ValidTo = validTo;
    }

    public void SetCanBeExchanged(bool canBeExchanged)
    {
        if (!canBeExchanged && CanBeExchanged && Supply != 0)
            throw new DisableExchangeabilityException();

        CanBeExchanged = canBeExchanged;
    }

    public bool CanBeRemoved() => Supply == 0;

}

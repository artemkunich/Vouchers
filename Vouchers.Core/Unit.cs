using System;
using System.Globalization;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class Unit : Entity<Guid>
    {
        public Guid UnitTypeId { get; }
        public UnitType UnitType { get; }

        public DateTime ValidFrom { get; private set; }
        public DateTime ValidTo { get; private set; }

        public bool CanBeExchanged { get; private set; }

        public decimal Supply { get; private set; }

        public static Unit Create(DateTime validFrom, DateTime validTo, bool canBeExchanged, UnitType value, CultureInfo cultureInfo = null) =>
            new Unit(Guid.NewGuid(), value, validFrom, validTo, canBeExchanged, 0, cultureInfo);

        private Unit(Guid id, UnitType unitType, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply, CultureInfo cultureInfo = null) : base(id)
        {
            if (validTo < DateTime.Today)
                throw new CoreException("ValidToIsLessThanToday", cultureInfo);

            if (validFrom > validTo)
                throw new CoreException("ValidFromIsGreaterThanValidTo", cultureInfo);

            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;

            UnitTypeId = unitType.Id;
            UnitType = unitType;
            
            Supply = supply;
        }

        private Unit() { }

        public void IncreaseSupply(decimal amount, CultureInfo cultureInfo = null)
        {
            if (amount <= 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);
            Supply += amount;

            UnitType.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount, CultureInfo cultureInfo = null)
        {
            if (amount <= 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);
            if (Supply < amount)
                throw new CoreException("AmountIsGreaterThanSupply", cultureInfo);
            Supply -= amount;

            UnitType.ReduceSupply(amount);
        }

        public void SetValidFrom(DateTime validFrom, CultureInfo cultureInfo = null)
        {
            if (validFrom > ValidFrom && Supply != 0)
                throw new CoreException("NewValidFromIsGreaterThanCurrentValidFrom", cultureInfo);

            if (validFrom > ValidTo)
                throw new CoreException("NewValidFromIsGreaterThanCurrentValidTo", cultureInfo);

            ValidFrom = validFrom;
        }

        public void SetValidTo(DateTime validTo, CultureInfo cultureInfo = null)
        {
            if (validTo < ValidTo && Supply != 0)
                throw new CoreException("NewValidToIsLessThanCurrentValidFrom", cultureInfo);

            if (ValidFrom > validTo)
                throw new CoreException("CurrentValidFromIsGreaterThanNewValidTo", cultureInfo);

            ValidTo = validTo;
        }

        public void SetCanBeExchanged(bool canBeExchanged, CultureInfo cultureInfo = null)
        {
            if (!canBeExchanged && CanBeExchanged && Supply != 0)
                throw new CoreException("CannotDisableExchangeability", cultureInfo);

            CanBeExchanged = canBeExchanged;
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

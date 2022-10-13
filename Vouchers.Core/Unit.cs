using System;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class Unit : Entity
    {
        public Guid UnitTypeId { get; }
        public UnitType UnitType { get; }

        public DateTime ValidFrom { get; private set; }
        public DateTime ValidTo { get; private set; }

        public bool CanBeExchanged { get; private set; }

        public decimal Supply { get; private set; }

        public static Unit Create(DateTime validFrom, DateTime validTo, bool canBeExchanged, UnitType value) =>
            new Unit(Guid.NewGuid(), value, validFrom, validTo, canBeExchanged, 0);

        internal Unit(Guid id, UnitType unitType, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply) : base(id)
        {
            if (validTo < DateTime.Today)
                throw new CoreException("ValidTo can not be before today");

            if (validFrom > validTo)
                throw new CoreException("ValidFrom can not be after validTo");

            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;

            UnitTypeId = unitType.Id;
            UnitType = unitType;
            
            Supply = supply;
        }

        private Unit() { }

        public void IncreaseSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            Supply += amount;

            UnitType.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            if (Supply < amount)
                throw new CoreException($"Detected attempt to set negative user's supply");
            Supply -= amount;

            UnitType.ReduceSupply(amount);
        }

        public void SetValidFrom(DateTime validFrom)
        {
            if (validFrom > ValidFrom && Supply != 0)
                throw new CoreException("New validFrom can not be after current validFrom");

            if (validFrom > ValidTo)
                throw new CoreException("ValidFrom can not be after validTo");

            ValidFrom = validFrom;
        }

        public void SetValidTo(DateTime validTo)
        {
            if (validTo < ValidTo && Supply != 0)
                throw new CoreException("New validTo can not be before current validFrom");

            if (ValidFrom > validTo)
                throw new CoreException("ValidFrom can not be after validTo");

            ValidTo = validTo;
        }

        public void SetCanBeExchanged(bool canBeExchanged)
        {
            if (!canBeExchanged && CanBeExchanged && Supply != 0)
                throw new CoreException("It is not allowed to switch off exchangeability");

            CanBeExchanged = canBeExchanged;
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

using System;
using System.Globalization;

namespace Vouchers.Core
{
    public sealed class UnitTypeQuantity
    {       
        public decimal Amount { get; }

        public Guid UnitTypeId { get; }
        public UnitType UnitType { get; }

        public static UnitTypeQuantity Create(decimal amount, UnitType unitType) =>
            new UnitTypeQuantity(amount, unitType);

        private UnitTypeQuantity(decimal amount, UnitType unit)
        {
            Amount = amount;
            UnitTypeId = unit.Id;
            UnitType = unit;
        }

        private UnitTypeQuantity() { }

        public UnitTypeQuantity Add(UnitQuantity unitQuantity, CultureInfo cultureInfo = null)
        {
            if (UnitType.NotEquals(unitQuantity.Unit.UnitType))
                throw new CoreException("CannotOperateWithDifferentUnitTypes", cultureInfo);

            return Create(Amount + unitQuantity.Amount, UnitType);
        }
    }

    

}

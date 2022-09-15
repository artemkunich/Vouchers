using System;

namespace Vouchers.Core
{
    public class UnitTypeQuantity
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

        public static UnitTypeQuantity operator + (UnitTypeQuantity q1, UnitTypeQuantity q2)
        {
            if (q1.UnitType.NotEquals(q2.UnitType))
                throw new CoreException("Cannot sum different voucher values");

            return Create(q1.Amount + q2.Amount, q1.UnitType);
        }

        public static UnitTypeQuantity operator +(UnitTypeQuantity q1, UnitQuantity q2)
        {
            if (q1.UnitType.NotEquals(q2.Unit.UnitType))
                throw new CoreException("Cannot sum different voucher values");

            return Create(q1.Amount + q2.Amount, q1.UnitType);
        }
    }

    

}

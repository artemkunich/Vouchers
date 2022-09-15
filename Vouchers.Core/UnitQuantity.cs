using System;

namespace Vouchers.Core
{
    public class UnitQuantity
    {       
        public decimal Amount { get; }

        public Guid UnitId { get; }
        public Unit Unit { get; }

        public static UnitQuantity Create(decimal amount, Unit unit)
        {
            return new UnitQuantity(amount, unit);
        }

        private UnitQuantity(decimal amount, Unit unit) 
        {
            Amount = amount;
            UnitId = unit.Id;
            Unit = unit;
        }

        private UnitQuantity() { }
    }
}

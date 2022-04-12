using System;

namespace Vouchers.Core
{
    public class VoucherValueQuantity
    {       
        public decimal Amount { get; }

        public VoucherValue Unit { get; }

        public static VoucherValueQuantity Create(decimal amount, VoucherValue unit)
        {
            return new VoucherValueQuantity(amount, unit);
        }

        public static VoucherValueQuantity operator + (VoucherValueQuantity q1, VoucherValueQuantity q2)
        {
            if (q1.Unit.NotEquals(q2.Unit))
                throw new CoreException("Cannot sum different voucher values");

            return Create(q1.Amount + q2.Amount, q1.Unit);
        }

        public static VoucherValueQuantity operator +(VoucherValueQuantity q1, VoucherQuantity q2)
        {
            if (q1.Unit.NotEquals(q2.Unit.Value))
                throw new CoreException("Cannot sum different voucher values");

            return Create(q1.Amount + q2.Amount, q1.Unit);
        }

        private VoucherValueQuantity(decimal amount, VoucherValue unit)
        {
            Amount = amount;
            Unit = unit;
        }

        private VoucherValueQuantity() { }
    }

    

}

using System;

namespace Vouchers.Core
{
    public class VoucherQuantity
    {       
        public decimal Amount { get; }
        
        public Voucher Unit { get; }

        public static VoucherQuantity Create(decimal amount, Voucher unit)
        {
            return new VoucherQuantity(amount, unit);
        }

        private VoucherQuantity(decimal amount, Voucher unit) 
        {
            Amount = amount;
            Unit = unit;
        }

        private VoucherQuantity() { }
    }
}

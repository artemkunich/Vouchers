using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class VoucherQuantity
    {
        public Voucher Unit { get; }

        public Action ChangeAmount;
        private decimal _Amount;
        public decimal Amount { 
            get {
                return _Amount;
            }
            set {
                _Amount = Math.Min(value,Unit.Balance);
                ChangeAmount?.Invoke();
            }
        }

        public VoucherQuantity(Voucher unit, decimal amount)
        {
            Unit = unit;
            _Amount = amount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class VoucherValueQuantity : INotifyPropertyChanged
    {
        public VoucherValue Unit { get; }

        private decimal _Amount;
        public decimal Amount  { 
            get { return _Amount; }
            set {
                _Amount = value;
                OnPropertyChanged();
            }
        }

        public VoucherValueQuantity(VoucherValue unit, decimal amount) {
            Unit = unit;
            Amount = amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

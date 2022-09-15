using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class Voucher : INotifyPropertyChanged 
    {
        public Voucher(DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply, decimal balance) {
            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;
            Supply = supply;
            Balance = balance;
        }

        public Voucher(int id, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply, decimal balance): this(validFrom, validTo, canBeExchanged, supply, balance)
        {
            Id = id;
        }

        private int _Id;
        public int Id 
        {
            get { return _Id; }
            set { 
                _Id = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ValidFrom;
        public DateTime ValidFrom
        {
            get { return _ValidFrom; }
            set {
                _ValidFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ValidTo;
        public DateTime ValidTo
        {
            get { return _ValidTo; }
            set
            {
                _ValidTo = value;
                OnPropertyChanged();
            }
        }

        private bool _CanBeExchanged;
        public bool CanBeExchanged
        {
            get { return _CanBeExchanged; }
            set
            {
                _CanBeExchanged = value;
                OnPropertyChanged();
            }
        }

        private decimal _Supply;
        public decimal Supply
        {
            get { return _Supply; }
            set
            {
                _Supply = value;
                OnPropertyChanged();
            }
        }

        private decimal _Balance;
        public decimal Balance
        {
            get { return _Balance; }
            set
            {
                _Balance = value;
                OnPropertyChanged();
            }
        }

        public Action IssueRedeem { get; set; }
        public DelegateCommand _IssueRedeemCommand;
        public DelegateCommand IssueRedeemCommand => _IssueRedeemCommand ??= new DelegateCommand(() => IssueRedeem?.Invoke());

        public Action Edit { get; set; }
        public DelegateCommand _EditCommand;
        public DelegateCommand EditCommand => _EditCommand ??= new DelegateCommand(() => Edit?.Invoke());

        public Action Remove { get; set; }
        public DelegateCommand _RemoveCommand;
        public DelegateCommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(() => Remove?.Invoke());


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}

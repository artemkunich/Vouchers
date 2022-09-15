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

    public class VoucherValue : INotifyPropertyChanged
    {
        public VoucherValue(string category, string name, string description, string issuerId, decimal supply, decimal balance, IEnumerable<Voucher> vouchers) {
            Category = category;
            Name = name;
            Description = description;
            IssuerId = issuerId;
            Supply = supply;
            Balance = balance;
            Vouchers = vouchers.ToList();
        }

        public VoucherValue(int id, string category, string name, string description, string issuerId, decimal supply, decimal balance, IEnumerable<Voucher> vouchers): this(category, name, description, issuerId, supply, balance, vouchers)
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

        private string _Category;
        public string Category
        {
            get { return _Category; }
            set {
                _Category = value;
                OnPropertyChanged();
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged();
            }
        }

        private string _IssuerId;
        public string IssuerId
        {
            get { return _IssuerId; }
            set
            {
                _IssuerId = value;
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

        public ICollection<Voucher> Vouchers { get; }
     
        public Action Edit { get; set; }
        public DelegateCommand _EditCommand;
        public DelegateCommand EditCommand => _EditCommand ??= new DelegateCommand(() => Edit?.Invoke());

        public Action Remove { get; set; }
        public DelegateCommand _RemoveCommand;
        public DelegateCommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(() => Remove?.Invoke());

        public Action CreateTransaction { get; set; }
        public DelegateCommand _CreateTransactionCommand;
        public DelegateCommand CreateTransactionCommand => _CreateTransactionCommand ??= new DelegateCommand(() => CreateTransaction?.Invoke());

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

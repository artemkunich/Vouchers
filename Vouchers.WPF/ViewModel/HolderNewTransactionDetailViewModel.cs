using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class HolderNewTransactionDetailViewModel : INotifyPropertyChanged
    {
        public string Header { get; }

        private string _ErrorMessage;
        public string ErrorMessage {
            get {
                return _ErrorMessage;
            }
            set {
                _ErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public string CreditorId { get; }

        private string _DebtorId;
        public string DebtorId { 
            get {
                return _DebtorId;
            }
            set {
                _DebtorId = value;
                UpdateItems();
            } 
        }
        public UserAccount AuthUser { get; set; }

        public VoucherValue Unit { get; }
        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<VoucherQuantity> Items { get; }

        public HolderNewTransactionDetailViewModel(string header, string creditorId, VoucherValue value) {
            Header = header;
            CreditorId = creditorId;
            Unit = value;
            Amount = 0m;
            Items = new ObservableCollection<VoucherQuantity>();
            UpdateItems();
        }

        private void UpdateItems() {
            Items.Clear();
            foreach (var voucher in Unit.Vouchers)
            {
                if (
                    CreditorId != Unit.IssuerId &&
                    DebtorId != Unit.IssuerId  &&
                    !voucher.CanBeExchanged
                ) continue;

                if (
                    DebtorId == Unit.IssuerId &&
                    voucher.ValidFrom > DateTime.Today
                ) continue;

                var voucherQuantity = new VoucherQuantity(voucher, 0);
                voucherQuantity.ChangeAmount += () =>
                {
                    Amount = Items.Select(item=>item.Amount).Aggregate((a1,a2) => a1 + a2);
                };
                Items.Add(voucherQuantity);
            }
        }

        public Action ProcessTransaction { get; set; }
        public DelegateCommand _ProcessTransactionCommand;
        public DelegateCommand ProcessTransactionCommand => _ProcessTransactionCommand ??= new DelegateCommand(
            () => ProcessTransaction.Invoke()
        );

        public Action Close { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

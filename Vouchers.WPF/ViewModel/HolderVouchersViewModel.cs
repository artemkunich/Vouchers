using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class HolderVouchersViewModel
    {
        public string Header { get; } = "Holder's vouchers";
        public MainViewModel mainViewModel;
        
        public HolderNewTransactionDetailViewModel TransactionDetailViewModel { get; private set; }

        UserAccount authUser;
        IServiceFactory serviceFactory;

        public ObservableCollection<VoucherValue> Values { get; private set; }
        public ObservableCollection<Voucher> Vouchers { get; set; }

        private VoucherValue _selectedValue;
        public VoucherValue SelectedValue {
            get { return _selectedValue; }
            set {
                _selectedValue = value;

                Vouchers.Clear();
                if (value == null) return;
                foreach (var voucher in value.Vouchers)
                {
                    Vouchers.Add(voucher);
                }
            } 
        }

        private Voucher _selectedVoucher;
        public Voucher SelectedVoucher {
            get { return _selectedVoucher; }
            set { _selectedVoucher = value; }
        }

        public Action Close { get; set; }
        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand => _CloseCommand ??= new DelegateCommand(() => { Close.Invoke();});

        public Action OpenTransactionDetail { get; set; }

        public void CreateTransaction() {

            var selectedValue = SelectedValue;
            if (selectedValue is null) return;

            var transactionDetailViewModel = new HolderNewTransactionDetailViewModel("New transaction", authUser.Id,selectedValue);

            transactionDetailViewModel.ProcessTransaction += () =>
            {
                try
                {
                    using (var accountingService = serviceFactory.CreateAccountingService())
                    {
                        accountingService.CreateHolderTransaction(
                            transactionDetailViewModel.CreditorId,
                            transactionDetailViewModel.DebtorId,
                            transactionDetailViewModel.Amount,
                            transactionDetailViewModel.Unit.Id,
                            transactionDetailViewModel.Items.Where(item => item.Amount != 0),
                            authUser
                        );
                    }
                    UpdateView(selectedValue.Id);
                    transactionDetailViewModel.Close?.Invoke();
                }
                catch (Exception ex) {
                    transactionDetailViewModel.ErrorMessage = ex.Message;
                }
            };
            TransactionDetailViewModel = transactionDetailViewModel;
            OpenTransactionDetail?.Invoke();
        }


        private DelegateCommand _UpdateCommand;
        public DelegateCommand UpdateCommand => _UpdateCommand ??= new DelegateCommand(() =>
        {
            var selectedValue = SelectedValue;

            if (selectedValue is null) {
                UpdateView();
                return;
            }

            var selectedValueId = selectedValue.Id;
            UpdateView(selectedValueId);
        });

        public void UpdateView(int valueId){
            UpdateView();
            SelectedValue = Values.Where(value => value.Id == valueId).FirstOrDefault();
        }

        public void UpdateView() {

            Vouchers.Clear();
            Values.Clear();

            using (var reportsService = serviceFactory.CreateReportsService())
            {
                var values = reportsService.GetHolderVoucherValues(authUser).ToList();
                
                foreach (var value in values)
                {
                    value.CreateTransaction += CreateTransaction;
                    Values.Add(value);
                }
            }
        }

        public HolderVouchersViewModel(UserAccount authUser, IServiceFactory serviceFactory)
        {
            this.authUser = authUser;
            this.serviceFactory = serviceFactory;

            Values = new ObservableCollection<VoucherValue>();
            Vouchers = new ObservableCollection<Voucher>();

            UpdateView();
        }
    }
}

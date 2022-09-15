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
    public class IssuerVouchersViewModel
    {
        public string Header { get; } = "Issuer's vouchers";
        public MainViewModel mainViewModel;

        public ValueDetailViewModel ValueDetailViewModel { get; private set; }
        public VoucherDetailViewModel VoucherDetailViewModel { get; private set; }
        public IssueRedeemViewModel IssueRedeemViewModel { get; private set; }

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

        #region Value

        public Action OpenValueDetail { get; set; }

        private DelegateCommand _CreateValueCommand;
        public DelegateCommand CreateValueCommand => _CreateValueCommand ??= new DelegateCommand(() => {          

            var valueDetailViewModel = new ValueDetailViewModel("New value", "", "", "");
            valueDetailViewModel.Save += () =>
            {
                try
                {
                    using (var valuesService = serviceFactory.CreateValuesService())
                    {
                        var createdValue = valuesService.CreateVoucherValue(
                            valueDetailViewModel.Category,
                            valueDetailViewModel.Name,
                            valueDetailViewModel.Description,
                            authUser);
                        createdValue.Edit += UpdateValue;
                        createdValue.Remove += RemoveValue;
                        Values.Add(createdValue);
                    }
                    valueDetailViewModel.Close?.Invoke();
                }
                catch (Exception ex) {
                    valueDetailViewModel.ErrorMessage = ex.Message;
                }
            };
            ValueDetailViewModel = valueDetailViewModel;
            OpenValueDetail?.Invoke(); 
        });
        
        public void UpdateValue(){

            var selectedValue = SelectedValue;
            if (selectedValue is null) return;

            var valueDetailViewModel = new ValueDetailViewModel("Edit value", selectedValue.Category, selectedValue.Name, selectedValue.Description);
            valueDetailViewModel.Save += () =>
            {
                try
                {
                    using (var valuesService = serviceFactory.CreateValuesService())
                    {
                        VoucherValue updatedValue = valuesService.UpdateVoucherValue(
                            selectedValue.Id,
                            valueDetailViewModel.Category,
                            valueDetailViewModel.Name,
                            valueDetailViewModel.Description,
                            authUser);

                        selectedValue.Category = updatedValue.Category;
                        selectedValue.Name = updatedValue.Name;

                        selectedValue.Description = updatedValue.Description;
                        selectedValue.Supply = updatedValue.Supply;
                    }
                    valueDetailViewModel.Close?.Invoke();
                }
                catch (Exception ex) {
                    valueDetailViewModel.ErrorMessage = ex.Message;
                }
            };

            ValueDetailViewModel = valueDetailViewModel;
            OpenValueDetail?.Invoke();
        }
        
        public void RemoveValue() {

            var selectedValue = SelectedValue;
            if (selectedValue is null) return;

            using (var valuesService = serviceFactory.CreateValuesService())
            {
                valuesService.RemoveVoucherValue(selectedValue.Id, authUser);
            }
            Values.Remove(selectedValue);
        }

        #endregion



        #region Voucher

        public Action OpenVoucherDetail { get; set; }
        public Action OpenIssueRedeemDetail { get; set; }

        private DelegateCommand _CreateVoucherCommand;
        public DelegateCommand CreateVoucherCommand => _CreateVoucherCommand ??= new DelegateCommand(() => {

            var selectedValue = SelectedValue;
            if (selectedValue is null) return;

            var voucherDetailViewModel = new VoucherDetailViewModel("New voucher", DateTime.Today, DateTime.MaxValue, false);

            voucherDetailViewModel.Save += () =>
            {
                try
                {
                    using (var valuesService = serviceFactory.CreateValuesService())
                    {
                        Voucher createdVoucher = valuesService.AddVoucher(
                            selectedValue.Id,
                            voucherDetailViewModel.ValidFrom,
                            voucherDetailViewModel.ValidTo,
                            voucherDetailViewModel.CanBeExchanged,
                            authUser);

                        createdVoucher.IssueRedeem += IssueRedeemVoucher;
                        createdVoucher.Edit += UpdateVoucher;
                        createdVoucher.Remove += RemoveVoucher;
                        Vouchers.Add(createdVoucher);
                    }
                    voucherDetailViewModel.Close?.Invoke();
                }
                catch (Exception ex) {
                    voucherDetailViewModel.ErrorMessage = ex.Message;
                }
            };
            VoucherDetailViewModel = voucherDetailViewModel;
            OpenVoucherDetail?.Invoke();
        });

        public void UpdateVoucher()
        {
            var selectedValue = SelectedValue;
            var selectedVoucher = SelectedVoucher;
            if (selectedValue is null || selectedVoucher is null) return;

            var voucherDetailViewModel = new VoucherDetailViewModel("Edit voucher", selectedVoucher.ValidFrom, selectedVoucher.ValidTo, selectedVoucher.CanBeExchanged);
            voucherDetailViewModel.Save += () =>
            {
                try
                {
                    using (var valuesService = serviceFactory.CreateValuesService())
                    {
                        Voucher updatedVoucher = valuesService.UpdateVoucher(
                            selectedValue.Id,
                            selectedVoucher.Id,
                            voucherDetailViewModel.ValidFrom,
                            voucherDetailViewModel.ValidTo,
                            voucherDetailViewModel.CanBeExchanged,
                            authUser);

                        selectedVoucher.ValidFrom = updatedVoucher.ValidFrom;
                        selectedVoucher.ValidTo = updatedVoucher.ValidTo;

                        selectedVoucher.CanBeExchanged = updatedVoucher.CanBeExchanged;
                        selectedValue.Supply = updatedVoucher.Supply;
                    }
                    voucherDetailViewModel.Close?.Invoke();
                }
                catch (Exception ex) {
                    voucherDetailViewModel.ErrorMessage = ex.Message;
                }
            };
            VoucherDetailViewModel = voucherDetailViewModel;
            OpenVoucherDetail?.Invoke();
        }

        public void RemoveVoucher()
        {
            var selectedValue = SelectedValue;
            var selectedVoucher = SelectedVoucher;
            if (selectedValue is null || selectedVoucher is null) return;

            using (var valuesService = serviceFactory.CreateValuesService())
            {
                valuesService.RemoveVoucher(selectedValue.Id, selectedVoucher.Id, authUser);
            }
            UpdateView(selectedValue.Id);
            Vouchers.Remove(selectedVoucher);
        }

        public void IssueRedeemVoucher() {

            var selectedValue = SelectedValue;
            var selectedVoucher = SelectedVoucher;
            if (selectedValue is null || selectedVoucher is null) return;

            var issueRedeemViewModel = new IssueRedeemViewModel("Issue/redeem voucher", 0, selectedVoucher.Balance);

            issueRedeemViewModel.Issue += (decimal amount) =>
            {
                issueRedeemViewModel.ErrorMessage = null;
                try
                {
                    using (var accountingService = serviceFactory.CreateAccountingService())
                    {
                        accountingService.CreateIssuerTransaction(amount, selectedVoucher.Id, authUser);
                    }
                    UpdateView(selectedValue.Id);
                }
                catch (Exception ex) {
                    issueRedeemViewModel.ErrorMessage = ex.Message;
                }
            };

            issueRedeemViewModel.Redeem += (decimal amount) =>
            {
                issueRedeemViewModel.ErrorMessage = null;
                try
                {
                    using (var accountingService = serviceFactory.CreateAccountingService())
                    {
                        accountingService.CreateIssuerTransaction(-amount, selectedVoucher.Id, authUser);
                    }
                    UpdateView(selectedValue.Id);
                }
                catch (Exception ex) {
                    issueRedeemViewModel.ErrorMessage = ex.Message;
                }
            };

            IssueRedeemViewModel = issueRedeemViewModel;
            OpenIssueRedeemDetail?.Invoke();
        }

        #endregion

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
                var values = reportsService.GetIssuerVoucherValues(authUser).ToList();
                
                foreach (var value in values)
                {                    
                    value.Edit += UpdateValue;
                    value.Remove += RemoveValue;

                    foreach (var voucher in value.Vouchers)
                    {
                        voucher.IssueRedeem += IssueRedeemVoucher;
                        voucher.Edit += UpdateVoucher;
                        voucher.Remove += RemoveVoucher;
                    }

                    Values.Add(value);
                }
            }
        }

        public IssuerVouchersViewModel(UserAccount authUser, IServiceFactory serviceFactory)
        {
            this.authUser = authUser;
            this.serviceFactory = serviceFactory;

            Values = new ObservableCollection<VoucherValue>();
            Vouchers = new ObservableCollection<Voucher>();

            UpdateView();
        }
    }
}

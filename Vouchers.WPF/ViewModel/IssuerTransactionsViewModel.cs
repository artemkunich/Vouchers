using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;
using Vouchers.WPF.Model.Specifications;

namespace Vouchers.WPF.ViewModel
{
    public class IssuerTransactionsViewModel
    {
        public string Header { get; } = "Issuer's transactions";
        public object SelectedTransaction { get; set; }

        UserAccount authUser;
        IServiceFactory serviceFactory;

        public decimal MinAmountFilter { get; set; }
        public decimal MaxAmountFilter { get; set; }
        public DateTime MinTimestampFilter { get; set; }
        public DateTime MaxTimestampFilter { get; set; }
        public string ValueCategoryFilter { get; set; }
        public string ValueNameFilter { get; set; }

        public ObservableCollection<object> Transactions { get; }

        private DelegateCommand _UpdateCommand;
        public DelegateCommand UpdateCommand => _UpdateCommand ??= new DelegateCommand(UpdateView);

        public void UpdateView()
        {
            Transactions.Clear();

            var specifications = new List<TransactionSpecification>();
            if (MinAmountFilter > 0)
                specifications.Add(new TransactionMinAmount(MinAmountFilter));

            if (MaxAmountFilter > 0)
                specifications.Add(new TransactionMaxAmount(MaxAmountFilter));

            specifications.Add(new TransactionTimestamp(MinTimestampFilter, MaxTimestampFilter));

            if (ValueCategoryFilter != "")
                specifications.Add(new TransactionValueCategory(ValueCategoryFilter));
            if (ValueNameFilter != "")
                specifications.Add(new TransactionValueName(ValueNameFilter));

            using (var reportsService = serviceFactory.CreateReportsService())
            {
                var transactions = reportsService.GetIssuerTransactions(specifications, authUser).ToList();

                foreach (var transaction in transactions)
                    Transactions.Add(
                        new {
                            Id = transaction.Id,
                            Timestamp = transaction.Timestamp,
                            Value = $"{transaction.UnitCategory}/{transaction.UnitName}",
                            Amount = transaction.Amount,
                            ValidFrom = transaction.ValidFrom,
                            ValidTo = transaction.ValidTo,
                            CanBeExchanged = transaction.CanBeExchanged
                        }
                    ); 
            }
        }

        public Action Close { get; set; }
        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand => _CloseCommand ??= new DelegateCommand(() => { Close.Invoke(); });

        public IssuerTransactionsViewModel(UserAccount authUser, IServiceFactory serviceFactory)
        {
            this.authUser = authUser;
            this.serviceFactory = serviceFactory;

            MinAmountFilter = 0;
            MaxAmountFilter = 0;
            MinTimestampFilter = DateTime.Today.AddMonths(-1);
            MaxTimestampFilter = DateTime.Today;
            ValueCategoryFilter = "";
            ValueNameFilter = "";

          Transactions = new ObservableCollection<object>();
        }
    }
}

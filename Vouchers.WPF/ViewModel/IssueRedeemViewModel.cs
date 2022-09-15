using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class IssueRedeemViewModel : INotifyPropertyChanged
    {
        public string Header { get; }

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get {
                return _ErrorMessage;
            }
            set {
                _ErrorMessage = value;
                OnPropertyChanged();
            }
        }


        public decimal IssueAmount { get; set; }
        public decimal RedeemAmount { get; set; }

        public Action<decimal> Issue { get; set; }
        public Action<decimal> Redeem { get; set; }

        public IssueRedeemViewModel(string header, decimal issueAmount, decimal redeemAmount) {
            Header = header;
            IssueAmount = issueAmount;
            RedeemAmount = redeemAmount;
        }
        
        private DelegateCommand _IssueCommand;
        public DelegateCommand IssueCommand => _IssueCommand ??= new DelegateCommand(()=> { Issue.Invoke(IssueAmount); });

        private DelegateCommand _RedeemCommand;
        public DelegateCommand RedeemCommand => _RedeemCommand ??= new DelegateCommand(() => { Redeem.Invoke(RedeemAmount); });

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

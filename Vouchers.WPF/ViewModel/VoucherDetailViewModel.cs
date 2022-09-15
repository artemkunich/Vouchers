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
    public class VoucherDetailViewModel : INotifyPropertyChanged
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

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool CanBeExchanged { get; set; }
        public Action Save { get; set; }
        public Action Close { get; set; }


        public VoucherDetailViewModel(string header, DateTime validFrom, DateTime validTo, bool canBeExchanged) {
            Header = header;
            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;
        }
        
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(()=> { Save.Invoke();});

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

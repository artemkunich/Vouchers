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
    public class ValueDetailViewModel : INotifyPropertyChanged
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


        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Action Save { get; set; }


        public ValueDetailViewModel(string header, string category, string name, string description) {
            Header = header;
            Category = category;
            Name = name;
            Description = description;
        }
        
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(()=> { Save.Invoke();});

        public Action Close { get; set; }

        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand => _CancelCommand ??= new DelegateCommand(Close);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

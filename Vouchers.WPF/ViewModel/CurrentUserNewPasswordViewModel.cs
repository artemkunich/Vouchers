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
    public class CurrentUserNewPasswordViewModel : INotifyPropertyChanged
    {
        private bool _FirstStep;
        public bool FirstStep {
            get {
                return _FirstStep;
            }
            set {                 
                _FirstStep = value;
                AuthCodeVisible = !value;
                SaveButtonText = value ? "Continue" : "Save";
            }
        }

        private bool _AuthCodeVisible;
        public bool AuthCodeVisible {
            get {
                return _AuthCodeVisible;
            }
            set {
                _AuthCodeVisible = value;
                OnPropertyChanged();
            }
        }

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _SaveButtonText;
        public string SaveButtonText
        {
            get
            {
                return _SaveButtonText;
            }
            set
            {
                _SaveButtonText = value;
                OnPropertyChanged();
            }
        }



        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string AuthCode { get; set; }

        public Action FirstStepConfirm { get; set; }
        public Action FirstStepDone { get; set; }

        public Action Save { get; set; }
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(() => Save?.Invoke());

        public Action Close { get; set; }
        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand => _CloseCommand ??= new DelegateCommand(() => Close?.Invoke());


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public CurrentUserNewPasswordViewModel() {
            FirstStep = true;
        }
    }
}
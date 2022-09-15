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
    public class NewUserViewModel : INotifyPropertyChanged
    {
        IServiceFactory ServiceFactory { get; set; }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AuthCode { get; set; }


        private bool _FirstStep;
        public bool FirstStep
        {
            get
            {
                return _FirstStep;
            }
            private set
            {
                _FirstStep = value;
                AuthCodeVisible = !value;
                ConfirmButtonText = value ? "Continue" : "Save";
            }
        }


        private bool _AuthCodeVisible;
        public bool AuthCodeVisible
        {
            get { return _AuthCodeVisible; }
            set
            {
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

        private string _ConfirmButtonText;
        public string ConfirmButtonText
        {
            get
            {
                return _ConfirmButtonText;
            }
            set
            {
                _ConfirmButtonText = value;
                OnPropertyChanged();
            }
        }

        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(SaveUser);

        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand => _CancelCommand ??= new DelegateCommand(Cancel);

        public Action Confirm { get; set; }
        public Action Close { get; set; }



        public NewUserViewModel(IServiceFactory serviceFactory) {
            ServiceFactory = serviceFactory;
            FirstStep = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void SaveUser() {
            ErrorMessage = null;
            try
            {
                if (FirstStep)
                {
                    Confirm?.Invoke();

                    if (Id is null || Email is null || Password is null || ConfirmPassword is null)
                    {
                        ErrorMessage = "Please fill in the form";
                        return;
                    }

                    if (Id.Length == 0 || Email.Length == 0 || Password.Length == 0 || ConfirmPassword.Length == 0)
                    {
                        ErrorMessage = "Please fill in the form";
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        ErrorMessage = "Passwords not matched";
                        return;
                    }

                    using (var userService = ServiceFactory.CreateUserService())
                    {
                        userService.SendCodeToCreateUserAccount(Id, Email, Password);
                        FirstStep = false;
                    }
                }
                else
                {
                    using (var userService = ServiceFactory.CreateUserService())
                    {
                        int authCode;
                        int.TryParse(AuthCode, out authCode);
                        var authUser = userService.CreateUserAccount(authCode, Email);
                        FirstStep = true;
                    }
                    Close?.Invoke();
                }
            }
            catch (Exception ex) {
                ErrorMessage = ex.Message;
            }
        }

        private void Cancel()
        {
            Close?.Invoke();
        }

    }
}

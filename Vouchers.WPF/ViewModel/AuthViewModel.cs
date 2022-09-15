using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private bool _FirstStep;
        public bool FirstStep { 
            get {
                return _FirstStep;
            }
            private set {
                _FirstStep = value;
                EmailVisible = value;
                PasswordVisible = !value;
                AuthCodeVisible = !value;
                ConfirmButtonText = value ? "Continue" : "Log in";
            } 
        }

        private bool _EmailVisible;
        public bool EmailVisible { 
            get { return _EmailVisible; }
            set { 
                _EmailVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _PasswordVisible;
        public bool PasswordVisible {
            get { return _PasswordVisible; }
            set
            {
                _PasswordVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _AuthCodeVisible;
        public bool AuthCodeVisible {
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
        public string ConfirmButtonText {
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

        public UserAccount AuthUser { get; private set; }
        public IServiceFactory ServiceFactory { get; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string AuthCode { get; set; }


        private DelegateCommand _CreateUserCommand;
        public DelegateCommand CreateUserCommand => _CreateUserCommand ??= new DelegateCommand(CreateUser);

        private DelegateCommand _LoginCommand;       
        public DelegateCommand LoginCommand => _LoginCommand ??= new DelegateCommand(Login);

        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand => _CancelCommand ??= new DelegateCommand(Cancel);

        public Action OpenMain { get; set; }
        public Action OpenNewUser { get; set; }      

        
        public Action FirstStepDone { get; set; }
        public Action SecondStepConfirm { get; set; }
        public Action Close { get; set; }

        public AuthViewModel(IServiceFactory serviceFactory) {
            ServiceFactory = serviceFactory;
            FirstStep = true;
        }

        public void CreateUser() {
            OpenNewUser?.Invoke();
        }

        public void Login()
        {
            ErrorMessage = null;
            try
            {
                if (FirstStep)
                {
                    using (var userService = ServiceFactory.CreateUserService())
                    {
                        userService.SendCodeToGetUserAccount(Email);
                        FirstStep = false;
                        FirstStepDone?.Invoke();
                    }
                }
                else
                {
                    SecondStepConfirm?.Invoke();

                    using (var userService = ServiceFactory.CreateUserService())
                    {
                        int authCode;
                        int.TryParse(AuthCode, out authCode);
                        AuthUser = userService.GetUserAccount(authCode, Email, Password);
                        FirstStep = true;
                    }
                    OpenMain?.Invoke();
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


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class AuthCommand : ICommand
    {
        Action _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod;

        public AuthCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public AuthCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                //throw new NotImplementedException();
            }

            remove
            {
                //throw new NotImplementedException();
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();

            if (_TargetExecuteMethod != null)
                return true;

            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
                _TargetExecuteMethod();
        }
    }

}

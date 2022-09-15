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
    public class CurrentUserViewModel : INotifyPropertyChanged
    {
        public string Header { get; }

        UserAccount AuthUser { get; }
        public string Id { get; }

        private string _Email;
        public string Email {
            get {
                return _Email;
            }
            set {
                _Email = value;
                OnPropertyChanged();
            } 
        }

        private DateTime _LastPassUpdate;
        public DateTime LastPassUpdate {
            get {
                return _LastPassUpdate;
            }
            private set {
                _LastPassUpdate = value;
                OnPropertyChanged();
            }
        }

        IServiceFactory serviceFactory;

        public Action Close { get; set; }
        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand => _CloseCommand ??= new DelegateCommand(() => { Close.Invoke(); });

        public Action<CurrentUserUpdateEmailViewModel> OpenUpdateEmail { get; set; }
        public DelegateCommand _UpdateEmailCommand;
        public DelegateCommand UpdateEmailCommand => _UpdateEmailCommand ??= new DelegateCommand(UpdateEmail);

        public Action<CurrentUserNewPasswordViewModel> OpenResetPassword { get; set; }
        public DelegateCommand _ResetPasswordCommand;
        public DelegateCommand ResetPasswordCommand => _ResetPasswordCommand ??= new DelegateCommand(ResetPassword);


        public CurrentUserViewModel(UserAccount authUser, IServiceFactory serviceFactory) {

            Header = "My account";
            AuthUser = authUser;
            Id = authUser.Id;
            Email = authUser.Email;
            LastPassUpdate = authUser.LastPassUpdate;
            this.serviceFactory = serviceFactory;
        }


        public void UpdateEmail()
        {
            var vm = new CurrentUserUpdateEmailViewModel();
            vm.Save += () =>
            {
                try
                {
                    vm.ErrorMessage = null;
                    using (var service = serviceFactory.CreateUserService())
                    {
                        if (vm.FirstStep)
                        {
                            if (vm.NewEmail?.Length == 0)
                            {
                                vm.ErrorMessage = "Please fill in the form";
                                return;
                            }

                            service.SendCodeToUpdateEmail(vm.NewEmail, AuthUser);
                            vm.FirstStep = false;
                            vm.FirstStepDone?.Invoke();
                        }
                        else
                        {
                            int authCode;
                            int.TryParse(vm.AuthCode, out authCode);
                            var updatedAuthUser = service.UpdateEmail(authCode, AuthUser);
                            AuthUser.Email = updatedAuthUser.Email;
                            Email = updatedAuthUser.Email;
                            vm.Close?.Invoke();
                        }
                    }
                }
                catch (Exception ex)
                {
                    vm.ErrorMessage = ex.Message;
                }
            };

            OpenUpdateEmail?.Invoke(vm);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        public void ResetPassword()
        {
            var vm = new CurrentUserNewPasswordViewModel();
            vm.Save += () =>
            {
                try
                {
                    vm.ErrorMessage = null;
                    using (var service = serviceFactory.CreateUserService())
                    {
                        if (vm.FirstStep)
                        {
                            vm.FirstStepConfirm?.Invoke();

                            if (
                                vm.CurrentPassword.ToString().Length == 0 || 
                                vm.NewPassword.ToString().Length == 0 || 
                                vm.ConfirmPassword.ToString().Length == 0
                            )
                            {
                                vm.ErrorMessage = "Please fill in the form";
                                return;
                            }


                            if (vm.NewPassword == vm.ConfirmPassword && vm.NewPassword.Length > 0)
                            {
                                service.SendCodeToResetPassword(vm.CurrentPassword, vm.NewPassword, AuthUser);
                                vm.FirstStep = false;
                                vm.FirstStepDone?.Invoke();
                            }
                            else
                                vm.ErrorMessage = "Passwords not matched";
                        }
                        else
                        {
                            int authCode;
                            int.TryParse(vm.AuthCode, out authCode);
                            var updatedAuthUser = service.ResetPassword(authCode, AuthUser);
                            AuthUser.LastPassUpdate = updatedAuthUser.LastPassUpdate;
                            LastPassUpdate = updatedAuthUser.LastPassUpdate;
                            AuthUser.PassHash = updatedAuthUser.PassHash;
                            vm.Close?.Invoke();
                        }
                    }
                }
                catch (Exception ex) {
                    vm.ErrorMessage = ex.Message;
                }

            };

            OpenResetPassword?.Invoke(vm);
        }

    }
}

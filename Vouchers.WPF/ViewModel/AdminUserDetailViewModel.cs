using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.ViewModel
{
    public class AdminUserDetailViewModel : INotifyPropertyChanged
    {
        public string Header { get; }

        private string _SuccessMessage;
        public string SuccessMessage
        {
            get { return _SuccessMessage; }
            set
            {
                _SuccessMessage = value;
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


        public bool IssuerOperations { get; set; }
        public bool HolderOperations { get; set; }
        public bool AdminOperations { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public Action UpdatePermissions { get; set; }
        public DelegateCommand _UpdatePermissionsCommand;
        public DelegateCommand UpdatePermissionsCommand => _UpdatePermissionsCommand ??= new DelegateCommand(
            () => UpdatePermissions.Invoke()
        );

        public Action ResetPasswordConfirm { get; set; }
        public Action ResetPassword { get; set; }
        public DelegateCommand _ResetPasswordCommand;
        public DelegateCommand ResetPasswordCommand => _ResetPasswordCommand ??= new DelegateCommand(
            () => ResetPassword.Invoke()
        );


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AdminUserDetailViewModel(
            bool issuerOperations, 
            bool holderOperations,
            bool adminOperations,
            string header
        ) {
            Header = header;
            IssuerOperations = issuerOperations;
            HolderOperations = holderOperations;
            AdminOperations = adminOperations;
        }
    }
}

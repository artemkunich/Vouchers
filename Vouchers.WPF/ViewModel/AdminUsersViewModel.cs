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
    public class AdminUsersViewModel
    {
        public string Header { get; } = "User accounts";

        UserAccount authUser;
        IServiceFactory serviceFactory;

        public string IdFilter { get; set; }
        public string EmailFilter { get; set; }

        public object SelectedUserAccount { get; set; }
        public ObservableCollection<object> UserAccounts { get; private set; }

        public Action<AdminUserDetailViewModel> OpenUserDetail { get; set; }

        private DelegateCommand _UpdateCommand;
        public DelegateCommand UpdateCommand => _UpdateCommand ??= new DelegateCommand(UpdateView);

        public Action Close { get; set; }
        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand => _CloseCommand ??= new DelegateCommand(() => { Close.Invoke(); });

        public AdminUsersViewModel(UserAccount authUser, IServiceFactory serviceFactory)
        {
            this.authUser = authUser;
            this.serviceFactory = serviceFactory;

            IdFilter = "";
            EmailFilter = "";

            UserAccounts = new ObservableCollection<object>();
        }


        public void UpdateView()
        {
            UserAccounts.Clear();

            using (var reportsService = serviceFactory.CreateReportsService())
            {
                var specifications = new List<UserAccountSpecification>();

                if (IdFilter != "")
                    specifications.Add(new UserAccountId(IdFilter));

                if (EmailFilter != "")
                    specifications.Add(new UserAccountEmail(EmailFilter));

                var userAccounts = reportsService.GetUserAccounts(specifications, authUser).ToList();

                foreach (var userAccount in userAccounts)
                {
                    UserAccounts.Add(
                        new {
                            Id = userAccount.Id,
                            Email = userAccount.Email,
                            IssuerOperations = userAccount.Permissions.IssuerOperations,
                            HolderOperations = userAccount.Permissions.HolderOperations,
                            AdminOperations = userAccount.Permissions.UserAdministration,
                            PasswordLastUpdate = userAccount.LastPassUpdate,

                            OpenUserDetailCommand = new DelegateCommand(
                                ()=> {
                                    var vm = new AdminUserDetailViewModel(
                                        userAccount.Permissions.IssuerOperations,
                                        userAccount.Permissions.HolderOperations,
                                        userAccount.Permissions.UserAdministration,
                                        $"User {userAccount.Id}"
                                    );
                                    vm.UpdatePermissions += () =>
                                    {
                                        vm.SuccessMessage = null;
                                        vm.ErrorMessage = null;
                                        try
                                        {
                                            using (var adminService = serviceFactory.CreateAdminService())
                                            {
                                                adminService.UpdatePermissions(
                                                    userAccount.Id,
                                                    new UserPermissions(vm.IssuerOperations, vm.HolderOperations, vm.AdminOperations),
                                                    authUser
                                                );
                                            }
                                            vm.SuccessMessage = "Permissions are changed";
                                            UpdateView();
                                        }
                                        catch (Exception ex) {
                                            vm.ErrorMessage = ex.Message;
                                        }
                                    };
                                    vm.ResetPassword += () =>
                                    {
                                        vm.ErrorMessage = null;
                                        try
                                        {
                                            vm.ResetPasswordConfirm?.Invoke();
                                            if (vm.NewPassword == "" || vm.ConfirmPassword == "") {
                                                vm.ErrorMessage = "Please fill in passwords";
                                                return;
                                            }                              

                                            if (vm.NewPassword != vm.ConfirmPassword)
                                            {
                                                vm.ErrorMessage = "Passwords not matched";
                                                return;
                                            }
                                            using (var adminService = serviceFactory.CreateAdminService())
                                            {
                                                adminService.ResetPassword(
                                                    userAccount.Id,
                                                    vm.NewPassword,
                                                    authUser
                                                );
                                            }
                                            vm.SuccessMessage = "Password is changed";
                                        }
                                        catch (Exception ex) {
                                            vm.ErrorMessage = ex.Message;
                                        }
                                    };

                                    OpenUserDetail?.Invoke(vm);
                                }
                            )
                        }
                    );
                }
            }
        }

    }
}

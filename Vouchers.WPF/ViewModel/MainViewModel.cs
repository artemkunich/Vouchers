using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows;

using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class MainViewModel
    {
        public UserAccount AuthUser { get; }

        IServiceFactory serviceFactory;

        public DelegateCommand _OpenIssuerVouchersCommand;
        public DelegateCommand OpenIssuerVouchersCommand => _OpenIssuerVouchersCommand ??= new DelegateCommand(OpenIssuerVouchers);

        public DelegateCommand _OpenIssuerTransactionsCommand;
        public DelegateCommand OpenIssuerTransactionsCommand => _OpenIssuerTransactionsCommand ??= new DelegateCommand(OpenIssuerTransactions);

        public DelegateCommand _OpenHolderVouchersCommand;
        public DelegateCommand OpenHolderVouchersCommand => _OpenHolderVouchersCommand ??= new DelegateCommand(OpenHolderVouchers);

        public DelegateCommand _OpenHolderTransactionsCommand;
        public DelegateCommand OpenHolderTransactionsCommand => _OpenHolderTransactionsCommand ??= new DelegateCommand(OpenHolderTransactions);

        public DelegateCommand _OpenAdminUserAccountsCommand;
        public DelegateCommand OpenAdminUserAccountsCommand => _OpenAdminUserAccountsCommand ??= new DelegateCommand(OpenAdminUserAccounts);

        public DelegateCommand _OpenUserAccountCommand;
        public DelegateCommand OpenUserAccountCommand => _OpenUserAccountCommand ??= new DelegateCommand(OpenUserAccount);

        public bool IssuerMenuVisible { get; }
        public bool HolderMenuVisible { get; }
        public bool AdminMenuVisible { get; }

        public ObservableCollection<object> TabItems { get; }

        public MainViewModel(UserAccount authUser, IServiceFactory serviceFactory)
        {
            AuthUser = authUser;

            IssuerMenuVisible = AuthUser.Permissions.IssuerOperations;
            HolderMenuVisible = AuthUser.Permissions.HolderOperations;
            AdminMenuVisible = AuthUser.Permissions.UserAdministration;

            this.serviceFactory = serviceFactory;

            TabItems = new ObservableCollection<object>();
        }

        public void OpenIssuerVouchers() {
            var vm = new IssuerVouchersViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }

        public void OpenIssuerTransactions()
        {
            var vm = new IssuerTransactionsViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }


        public void OpenHolderVouchers()
        {
            var vm = new HolderVouchersViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }

        public void OpenHolderTransactions()
        {
            var vm = new HolderTransactionsViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }
       
        public void OpenAdminUserAccounts()
        {
            var vm = new AdminUsersViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }

        public void OpenUserAccount()
        {
            var vm = new CurrentUserViewModel(AuthUser, serviceFactory);
            vm.Close += () => { TabItems.Remove(vm); };

            TabItems.Add(vm);
        }
    }
}

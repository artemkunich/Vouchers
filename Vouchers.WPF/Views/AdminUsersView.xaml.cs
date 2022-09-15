using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vouchers.WPF.ViewModel;

namespace Vouchers.WPF.Views
{
    /// <summary>
    /// Interakční logika pro AdminUserAccountsView.xaml
    /// </summary>
    public partial class AdminUsersView : UserControl
    {
        public AdminUsersView()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            IdFilter.Focus();

            if (DataContext is AdminUsersViewModel vm) {
                vm.OpenUserDetail ??= (vm) =>
                {
                    var userDetailWindow = new AdminUserDetailWindow();
                    userDetailWindow.DataContext = vm;
                    userDetailWindow.ShowDialog();

                };
                    
            }
        }
    }
}

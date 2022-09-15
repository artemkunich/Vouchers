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
using System.Windows.Shapes;
using Vouchers.WPF.ViewModel;

namespace Vouchers.WPF.Views
{
    /// <summary>
    /// Interakční logika pro AdminUserDetailWindow.xaml
    /// </summary>
    public partial class AdminUserDetailWindow : Window
    {
        public AdminUserDetailWindow()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            NewPassword.Focus();
            if (DataContext is AdminUserDetailViewModel vm)
            {
                vm.ResetPasswordConfirm += () =>
                {
                    vm.NewPassword = NewPassword.Password;
                    vm.ConfirmPassword = ConfirmPassword.Password;
                };
            }
        }
    }
}

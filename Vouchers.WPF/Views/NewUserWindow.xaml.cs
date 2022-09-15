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
    /// Interakční logika pro NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        public void View_Loaded(object sender, RoutedEventArgs e)
        {
            IdTextBox.Focus();

            if (DataContext is NewUserViewModel vm)
            {
                vm.Confirm += () =>
                {
                    vm.Password = Password.Password;
                    vm.ConfirmPassword = ConfirmPassword.Password;
                };
                vm.Close += () => Close();
            }
        }
    }
}

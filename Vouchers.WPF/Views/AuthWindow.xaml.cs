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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {

        public AuthWindow()
        {
            InitializeComponent();
            Loaded += AuthWindow_Loaded;
        }

        private void AuthWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Email.Focus();

            if (DataContext is AuthViewModel vm) {

                
                vm.FirstStepDone += () => Password.Focus();
                vm.SecondStepConfirm += () => vm.Password = Password.Password;
                vm.Close += () => Close();
                vm.OpenMain += () =>
                {
                    var mainWindow = new MainWindow();
                    mainWindow.DataContext = new MainViewModel(vm.AuthUser, vm.ServiceFactory); ;
                    mainWindow.Show();
                };
                vm.OpenNewUser += () =>
                {
                    var newUserWindow = new NewUserWindow();
                    newUserWindow.DataContext = new NewUserViewModel(vm.ServiceFactory);
                    newUserWindow.ShowDialog();
                };
            }
        }
    }
}

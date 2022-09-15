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
    /// Interakční logika pro ValueDetailWindow.xaml
    /// </summary>
    public partial class ValueDetailWindow : Window
    {
        public ValueDetailWindow()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ValueDetailViewModel vm)
            {
                vm.Close ??= () => Close();
            }
        }
    }
}

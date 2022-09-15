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
    /// Interakční logika pro HolderVouchersView.xaml
    /// </summary>
    public partial class HolderVouchersView : UserControl
    {
        public HolderVouchersView()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HolderVouchersViewModel vm)
            {
                if (vm.OpenTransactionDetail is null)
                    vm.OpenTransactionDetail += () =>
                    {
                        var dialog = new HolderNewTransactionDetailWindow();
                        dialog.DataContext = vm.TransactionDetailViewModel;
                        dialog.ShowDialog();
                    };               
            }
        }
    }
}

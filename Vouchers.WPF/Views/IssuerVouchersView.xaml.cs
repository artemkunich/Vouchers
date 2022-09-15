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
    /// Interakční logika pro IssuerVouchersView.xaml
    /// </summary>
    /// 
  
    public partial class IssuerVouchersView : UserControl
    {
        public IssuerVouchersView()
        {
            InitializeComponent();          
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IssuerVouchersViewModel vm)
            {
                vm.OpenValueDetail ??= () =>
                {
                    var dialog = new ValueDetailWindow();
                    dialog.DataContext = vm.ValueDetailViewModel;
                    dialog.ShowDialog();
                };

                vm.OpenVoucherDetail ??= () =>
                {
                    var dialog = new VoucherDetailWindow();
                    dialog.DataContext = vm.VoucherDetailViewModel;
                    dialog.ShowDialog();
                };

                vm.OpenIssueRedeemDetail ??= () =>
                {
                    var dialog = new IssueRedeemWindow();
                    dialog.DataContext = vm.IssueRedeemViewModel;
                    dialog.ShowDialog();
                };
            }
        }

    }
}

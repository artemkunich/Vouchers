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

namespace Vouchers.WPF.Views
{
    /// <summary>
    /// Interakční logika pro IssuerTransactionsView.xaml
    /// </summary>
    public partial class IssuerTransactionsView : UserControl
    {
        public IssuerTransactionsView()
        {
            InitializeComponent();
            Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            MinAmountFilter.Focus();
        }
    }
}

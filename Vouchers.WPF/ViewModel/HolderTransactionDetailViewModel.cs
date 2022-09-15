using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.ViewModel
{
    public class HolderTransactionDetailViewModel
    {
        public string Header { get; }

        public HolderTransaction Transaction { get; }

        public HolderTransactionDetailViewModel(string header, HolderTransaction transaction) {
            Header = header;
            Transaction = transaction;
        }
    }
}

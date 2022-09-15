using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionValueCategory : TransactionSpecification
    {
        public string Category { get; }

        public TransactionValueCategory(string category) {
            Category = category;
        }
    }
}

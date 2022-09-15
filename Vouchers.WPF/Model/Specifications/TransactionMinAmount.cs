using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionMinAmount : TransactionSpecification
    {
        public decimal MinAmount { get; }

        public TransactionMinAmount(decimal minAmount) {
            MinAmount = minAmount;
        }
    }
}

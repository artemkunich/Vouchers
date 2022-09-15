using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionMaxAmount : TransactionSpecification
    {
        public decimal MaxAmount { get; }

        public TransactionMaxAmount(decimal maxAmount) {
            MaxAmount = maxAmount;
        }
    }
}

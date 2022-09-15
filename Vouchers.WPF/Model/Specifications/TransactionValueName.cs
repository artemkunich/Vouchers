using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionValueName : TransactionSpecification
    {
        public string Name { get; }

        public TransactionValueName(string name) {
            Name = name;
        }
    }
}

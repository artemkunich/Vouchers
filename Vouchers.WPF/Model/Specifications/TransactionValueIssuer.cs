using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionValueIssuer : TransactionSpecification
    {
        public string IssuerId { get; }

        public TransactionValueIssuer(string issuerId) {
            IssuerId = issuerId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionCounterparty : TransactionSpecification
    {
        public string CounterpartyId { get; }

        public TransactionCounterparty(string counterpartyId) {
            CounterpartyId = counterpartyId;
        }
    }
}

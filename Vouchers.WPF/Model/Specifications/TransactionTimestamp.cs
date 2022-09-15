using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class TransactionTimestamp : TransactionSpecification
    {
        public DateTime MinTimestamp { get; }
        public DateTime MaxTimestamp { get; }

        public TransactionTimestamp(DateTime minTimestamp, DateTime maxTimestamp)
        {
            MinTimestamp = minTimestamp;
            MaxTimestamp = maxTimestamp;
        }
    }
}

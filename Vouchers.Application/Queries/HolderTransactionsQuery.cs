using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries
{
    public class HolderTransactionsQuery
    {
        public string Ticker { get; set; }
        public string IssuerName { get; set; }

        public string Creditor { get; set; }
        public string Debtor { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public DateTime? MinTimestamp { get; set; }
        public DateTime? MaxTimestamp { get; set; }
    }
}

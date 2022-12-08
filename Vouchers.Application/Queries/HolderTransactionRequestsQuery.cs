using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries
{
    public sealed class HolderTransactionRequestsQuery : ListQuery
    {
        public string Ticker { get; set; }
        public string IssuerName { get; set; }
        public string CounterpartyName { get; set; }

        public bool IncludeIncoming { get; set; }
        public bool IncludeOutgoing { get; set; }

        public bool IncludePerformed { get; set; }
        public bool IncludeNotPerformed { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public DateTime? MinDueDate { get; set; }
        public DateTime? MaxDueDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries
{
    public sealed class HolderTransactionsQuery : ListQuery
    {
        [Required]
        public Guid AccountId { get; set; } 
        public string Ticker { get; set; }
        public string IssuerName { get; set; }

        public string CounterpartyName { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public DateTime? MinTimestamp { get; set; }
        public DateTime? MaxTimestamp { get; set; }
    }
}

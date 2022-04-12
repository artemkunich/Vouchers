using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries
{
    public class DomainOffersQuery : Query
    {
        public string Name { get; set; }

        public int? MinMaxSubscribersCount { get; set; }
        public int? MaxMaxSubscribersCount { get; set; }

        public int? InvoicePeriod { get; set; }
        public int? Currency { get; set; }
        public decimal? MaxAmount { get; set; }
        public decimal? MinAmount { get; set; }
    }
}

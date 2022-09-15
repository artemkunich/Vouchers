using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries
{
    public class IssuerValuesQuery
    {
        [Required]
        public Guid IssuerDomainAccountId { get; set; }

        public string Ticker { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Vouchers.Core;

namespace Vouchers.Application.Commands
{
    public class CreateIssuerTransactionCommand
    {
        [Required]
        public Guid IssuerDomainAccountId { get; set; }

        [Required]
        public Guid VoucherId { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}

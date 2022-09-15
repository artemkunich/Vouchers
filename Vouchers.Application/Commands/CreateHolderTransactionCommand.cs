using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using Vouchers.Core;

namespace Vouchers.Application.Commands
{
    public class CreateHolderTransactionCommand
    {
        [Required]
        public Guid CreditorDomainAccountId { get; set; }
        [Required]
        public Guid DebtorDomainAccountId { get; set; }

        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public Guid VoucherValueId { get; set; }

        [Required]
        public ICollection<Tuple<Guid, decimal>> Items { get; set; }
    }
}

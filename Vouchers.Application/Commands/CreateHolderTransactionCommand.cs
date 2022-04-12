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
        public Guid CreditorDomainAccountId { get; }
        [Required]
        public Guid DebtorDomainAccountId { get; }

        [Required]
        public decimal Quantity { get; }
        [Required]
        public Guid VoucherValueId { get; }

        [Required]
        public ICollection<Tuple<decimal, Guid>> TransactionItems { get; }

        public void AddItem(decimal quantity, Guid voucherId) =>
            TransactionItems.Add(new Tuple<decimal, Guid>(quantity, voucherId));

        public CreateHolderTransactionCommand()
        {
            TransactionItems = new List<Tuple<decimal, Guid>>();
        }
    }
}

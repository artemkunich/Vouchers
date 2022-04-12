using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Commands
{
    public class CreateCollectionRequestCommand
    {
        [Required]
        public Guid CreditorDomainAccountId { get; }
        [Required]
        public Guid DebtorDomainAccountId { get; }

        [Required]
        public decimal Quantity { get; }
        [Required]
        public Guid VoucherValueId { get; }
        
        public DateTime MaxValidFrom { get; }
        public DateTime MinValidTo { get; }

        public bool MustBeExchangeable { get; }

        public DateTime ValidTo { get; }
    }
}

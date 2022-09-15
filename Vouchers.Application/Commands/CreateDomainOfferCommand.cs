using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class CreateDomainOfferCommand
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int MaxSubscribersCount { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string InvoicePeriod { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public int? MaxContractsPerIdentity { get; set; }
    }
}

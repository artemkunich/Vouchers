using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class DomainOfferDto
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxSubscribersCount { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string InvoicePeriod { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public sealed class DomainOfferDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxMembersCount { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string InvoicePeriod { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public int? MaxContractsPerIdentity { get; set; }

        public int? ContractsPerIdentity { get; set; }
    }
}

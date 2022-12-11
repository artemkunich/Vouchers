using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainOfferCommands
{
    public sealed class UpdateDomainOfferCommand
    {
        [Required]
        public Guid Id { get; set; }

        public bool? Terminate { get; set; }

        public string Description { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public int? MaxContractsPerIdentity { get; set; }
    }
}

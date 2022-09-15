using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class CreateDomainCommand
    {
        [Required]
        public Guid OfferId { get; set; }

        [Required]
        public string DomainName { get; set; }
    }
}

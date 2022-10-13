using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainAccountCommands
{
    public sealed class UpdateDomainAccountCommand
    {      
        [Required]
        public Guid DomainAccountId { get; set; }

        public bool? IsConfirmed { get; set; }

        public bool? IsIssuer { get; set; }

        public bool? IsAdmin { get; set; }
    }
}

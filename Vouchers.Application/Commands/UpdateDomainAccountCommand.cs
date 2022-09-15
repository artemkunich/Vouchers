using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class UpdateDomainAccountCommand
    {      
        [Required]
        public Guid DomainAccountId { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class CreateDomainAccountCommand
    {      
        [Required]
        public Guid DomainId { get; set; }
    }
}

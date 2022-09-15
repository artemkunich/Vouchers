using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.UseCases
{
    public class CreateIdentityCommand
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public IdentityDetailDto IdentityDetail { get; set; }
    }
}

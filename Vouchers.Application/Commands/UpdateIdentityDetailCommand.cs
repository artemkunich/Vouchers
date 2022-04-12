using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.UseCases
{
    public class UpdateIdentityDetailCommand
    {
        [Required]
        public Guid IdentityId { get; set; }

        [Required]
        public IdentityDetailDto IdentityDetailDto { get; set; }
    }
}

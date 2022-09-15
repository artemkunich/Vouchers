using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class UpdateIdentityCommand
    {
        [Required]
        public Guid IdentityId { get; set; }

        [Required]
        public IdentityDetailDto IdentityDetail { get; set; }

    }
}

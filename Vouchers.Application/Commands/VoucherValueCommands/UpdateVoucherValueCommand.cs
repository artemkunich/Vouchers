using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands
{
    public sealed class UpdateVoucherValueCommand
    {
        [Required]
        public Guid VoucherValueId { get; set; }

        [Required]
        public VoucherValueDetailDto VoucherValueDetail { get; set; }

        public IFormFile Image { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class UpdateVoucherValueCommand
    {
        [Required]
        public Guid VoucherValueId { get; set; }

        [Required]
        public VoucherValueDetailDto VoucherValueDetail { get; set; }

        public IFormFile Image { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class CreateVoucherValueCommand
    {
        [Required]
        public Guid IssuerDomainAccountId { get; set; }

        [Required]
        public VoucherValueDetailDto VoucherValueDetail { get; set; }

        public IFormFile Image { get; set; }
    }
}

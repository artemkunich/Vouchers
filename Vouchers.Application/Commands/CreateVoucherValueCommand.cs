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
        public Guid IssuerDomainAccountId { get; }

        [Required]
        public string Ticker { get; }

        [Required]
        public VoucherValueDetailDto VoucherValueDetailDto { get; }
    }
}

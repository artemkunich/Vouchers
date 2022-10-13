using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherCommands
{
    public sealed class UpdateVoucherCommand
    {
        [Required]
        public Guid VoucherValueId { get; set; }

        [Required]
        public VoucherDto Voucher { get; set; }   
    }
}

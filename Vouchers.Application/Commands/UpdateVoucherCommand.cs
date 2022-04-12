using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class UpdateVoucherCommand
    {
        [Required]
        public Guid VoucherId { get; }

        [Required]
        public VoucherDto VoucherDto { get; }   
    }
}

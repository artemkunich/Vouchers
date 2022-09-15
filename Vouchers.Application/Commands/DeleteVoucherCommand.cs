using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class DeleteVoucherCommand
    {
        [Required]
        public Guid VoucherValueId { get; }

        [Required]
        public Guid VoucherId { get; }
    }
}

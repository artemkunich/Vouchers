﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands
{
    public class UpdateVoucherValueDetailCommand
    {
        [Required]
        public Guid VoucherValueId { get; }

        [Required]
        public VoucherValueDetailDto VoucherValueDetailDto { get; set; }
    }
}

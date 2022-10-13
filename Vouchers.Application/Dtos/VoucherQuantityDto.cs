using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public sealed class VoucherQuantityDto
    {
        public decimal Amount { get; set; }
        public VoucherDto Unit { get; set; }     
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public sealed class VoucherValueQuantityDto
    {
        public VoucherValueDto Unit { get; set; }
        public decimal Amount { get; set; }
    }
}

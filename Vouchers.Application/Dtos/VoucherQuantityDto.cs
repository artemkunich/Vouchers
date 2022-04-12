using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class VoucherQuantityDto
    {
        public decimal Amount { get; set; }
        public VoucherDto Unit { get; set; }     
    }
}

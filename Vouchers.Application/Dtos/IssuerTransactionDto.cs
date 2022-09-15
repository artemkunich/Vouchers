using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class IssuerTransactionDto
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string UnitTicker { get; set; }

        public VoucherDto Unit { get; set; }

        public decimal Amount { get; set; }
    }
}

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

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool CanBeExchanged { get; set; }

        public decimal Amount { get; set; }
    }
}

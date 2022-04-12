using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class HolderTransactionDto
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid CreditorId { get; set; }
        public string CreditorName { get; set; }

        public Guid DebtorId { get; set; }
        public string DebtorName { get; set; }

        public Guid UnitId { get; set; }
        public string UnitTicker { get; set; }
        public Guid UnitIssuerId { get; set; }

        public decimal Amount { get; set; }

        public IEnumerable<VoucherQuantityDto> Items { get; set; }
    }
}

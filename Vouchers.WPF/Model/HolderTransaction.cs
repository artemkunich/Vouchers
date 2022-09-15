using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class HolderTransaction
    {
        public int Id { get; }

        public DateTime Timestamp { get; }

        public string CreditorId { get; }
        public string DebtorId { get; }

        public string UnitCategory { get; }
        public string UnitName { get; }
        public string UnitIssuerId { get; }

        public decimal Amount { get; }

        public ICollection<VoucherQuantity> Items { get; }

        public HolderTransaction(string creditorId, string debtorId, string unitCategory, string unitName, string unitIssuerId, decimal amount, IEnumerable<VoucherQuantity> items)
        {
            CreditorId = creditorId;
            DebtorId = debtorId;

            UnitCategory = unitCategory;
            UnitName = unitName;
            UnitIssuerId = unitIssuerId;

            Amount = amount;
            Items = items.ToList();
        }

        public HolderTransaction(DateTime timestamp, string creditorId, string debtorId, string unitCategory, string unitName, string unitIssuerId,  decimal amount, IEnumerable<VoucherQuantity> items) : this(creditorId, debtorId, unitCategory, unitName, unitIssuerId, amount, items)
        {
            Timestamp = timestamp;
        }

        public HolderTransaction(int id, DateTime timestamp, string creditorId, string debtorId, string unitCategory, string unitName, string unitIssuerId, decimal amount, IEnumerable<VoucherQuantity> items) : this(timestamp, creditorId, debtorId, unitCategory, unitName, unitIssuerId, amount, items)
        {
            Id = id;
        }
    }
}

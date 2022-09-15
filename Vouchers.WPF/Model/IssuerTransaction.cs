using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.WPF.Model
{
    public class IssuerTransaction
    {
        public int Id { get; }

        public DateTime Timestamp { get; private set; }

        public string UnitCategory { get; }
        public string UnitName { get; }

        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }

        public bool CanBeExchanged { get; }

        public decimal Amount { get; }

        public IssuerTransaction(int id, DateTime timestamp, string unitCategory, string unitName, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal amount)
        {
            Id = id;
            Timestamp = timestamp;
            UnitCategory = unitCategory;
            UnitName = unitName;
            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;
            Amount = amount;
        }
    }
}

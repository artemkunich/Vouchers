using System;
using System.Globalization;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class UnitType : Entity
    {
        public Guid IssuerAccountId { get; }
        public Account IssuerAccount { get; }
        public decimal Supply { get; private set; }

        public static UnitType Create(Account issuerAccount) =>
            new UnitType(Guid.NewGuid(), issuerAccount);

        private UnitType(Guid id, Account issuerAccount) : base(id)
        {
            IssuerAccountId = issuerAccount.Id;
            IssuerAccount = issuerAccount;
        }    

        private UnitType() { }

        public void IncreaseSupply(decimal amount, CultureInfo cultureInfo = null)
        {
            if (amount <= 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);
            Supply += amount;

            IssuerAccount.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount, CultureInfo cultureInfo = null)
        {
            if (amount <= 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);
            if (Supply < amount)
                throw new CoreException("AmountIsGreaterThanSupply", cultureInfo);
            Supply -= amount;

            IssuerAccount.ReduceSupply(amount);
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

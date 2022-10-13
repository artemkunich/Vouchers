using System;
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

        internal UnitType(Guid id, Account issuerAccount) : base(id)
        {
            IssuerAccountId = issuerAccount.Id;
            IssuerAccount = issuerAccount;
        }    

        private UnitType() { }

        public void IncreaseSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            Supply += amount;

            IssuerAccount.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            if (Supply < amount)
                throw new CoreException($"Detected attempt to set negative user's supply");
            Supply -= amount;

            IssuerAccount.ReduceSupply(amount);
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

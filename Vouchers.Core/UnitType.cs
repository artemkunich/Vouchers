using System;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class UnitType : Entity
    {
        public Guid IssuerId { get; }
        public Account Issuer { get; }
        public decimal Supply { get; private set; }

        public static UnitType Create(Account issuer) =>
            new UnitType(Guid.NewGuid(), issuer);

        internal UnitType(Guid id, Account issuer) : base(id)
        {
            IssuerId = issuer.Id;
            Issuer = issuer;
        }    

        private UnitType() { }

        public void IncreaseSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            Supply += amount;

            Issuer.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            if (Supply < amount)
                throw new CoreException($"Detected attempt to set negative user's supply");
            Supply -= amount;

            Issuer.ReduceSupply(amount);
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

using System;

namespace Vouchers.Core
{
    public class VoucherValue {

        public Guid Id { get; }

        public DomainAccount Issuer { get; }
        public decimal Supply { get; private set; }

        public static VoucherValue Create(DomainAccount issuer) =>
            new VoucherValue(Guid.NewGuid(), issuer);

        internal VoucherValue(Guid id, DomainAccount issuer) : this(issuer) =>
            Id = id;

        internal VoucherValue(DomainAccount issuer) => 
            Issuer = issuer;

        private VoucherValue() { }

        public bool Equals(VoucherValue voucherValue)
        {
            return Id == voucherValue.Id;
        }

        public bool NotEquals(VoucherValue voucherValue)
        {
            return Id != voucherValue.Id;
        }

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

using System;

namespace Vouchers.Core
{
    public class Voucher
    {
        public Guid Id { get; }

        public VoucherValue Value { get; }

        public DateTime ValidFrom { get; private set; }
        public DateTime ValidTo { get; private set; }

        public bool CanBeExchanged { get; private set; }

        public decimal Supply { get; private set; }

        public static Voucher Create(DateTime validFrom, DateTime validTo, bool canBeExchanged, VoucherValue value) =>
            new Voucher(Guid.NewGuid(), value, validFrom, validTo, canBeExchanged, 0);

        internal Voucher(Guid id, VoucherValue value, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply) : this(value, validFrom, validTo, canBeExchanged, supply) =>
            Id = id;

        internal Voucher(VoucherValue value, DateTime validFrom, DateTime validTo, bool canBeExchanged, decimal supply) 
        {
            if (validTo < DateTime.Today)
                throw new CoreException("ValidTo can not be before today");

            if (validFrom > validTo)
                throw new CoreException("ValidFrom can not be after validTo");

            ValidFrom = validFrom;
            ValidTo = validTo;
            CanBeExchanged = canBeExchanged;
            Value = value;
            Supply = supply;
        }

        

        private Voucher() { }

        public bool Equals(Voucher voucher) {
            return Id == voucher.Id;
        }

        public bool NotEquals(Voucher voucher) {
            return Id != voucher.Id;
        }

        public void IncreaseSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            Supply += amount;

            Value.IncreaseSupply(amount);
        }

        public void ReduceSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            if (Supply < amount)
                throw new CoreException($"Detected attempt to set negative user's supply");
            Supply -= amount;

            Value.ReduceSupply(amount);
        }

        public void SetValidFrom(DateTime validFrom)
        {
            if (validFrom > ValidFrom)
                throw new CoreException("New validFrom can not be after current validFrom");
            ValidFrom = validFrom;
        }

        public void SetValidTo(DateTime validTo)
        {
            if (validTo < ValidTo)
                throw new CoreException("New validTo can not be before current validFrom");
            ValidTo = validTo;
        }

        public void SetCanBeExchanged(bool canBeExchanged)
        {
            if (!canBeExchanged && CanBeExchanged && Supply != 0)
                throw new CoreException("It is not allowed to switch off exchangeability");

            CanBeExchanged = canBeExchanged;
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}

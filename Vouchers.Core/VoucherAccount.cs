using System;

namespace Vouchers.Core
{
    public class VoucherAccount
    {
        public Guid Id { get; }

        public DomainAccount Holder { get; }

        public decimal Balance { get; protected set; }

        public Voucher Unit { get; }

        public static VoucherAccount Create(DomainAccount holder, decimal balance, Voucher unit) =>
            new VoucherAccount(Guid.NewGuid(), holder, balance, unit);

        internal VoucherAccount(Guid id, DomainAccount holder, decimal balance, Voucher unit) : this(holder, balance, unit) =>
            Id = id;

        internal VoucherAccount(DomainAccount holder, decimal balance, Voucher unit) {
            Holder = holder;
            Balance = balance;
            Unit = unit;
        }

        private VoucherAccount() { }

        public bool Equals(VoucherAccount account) {
            return Id == account.Id;
        }

        public void ProcessDebit(decimal amount)
        {
            Balance += amount;
        }
        public void ProcessCredit(decimal amount)
        {
            if (amount > Balance)
            {
                throw new CoreException("Attempt to set negative balance");
            }
            Balance -= amount;
        }
    }
}

using System;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class AccountItem : Entity
    {
        public Guid HolderAccountId { get; }
        public Account HolderAccount { get; }

        public decimal Balance { get; private set; }

        public Guid UnitId { get; }
        public Unit Unit { get; }

        public static AccountItem Create(Account holderAccount, decimal balance, Unit unit) =>
            new AccountItem(Guid.NewGuid(), holderAccount, balance, unit);

        internal AccountItem(Guid id, Account holderAccount, decimal balance, Unit unit) : base(id)
        {
            HolderAccountId = holderAccount.Id;
            HolderAccount = holderAccount;
            
            Balance = balance;

            UnitId = unit.Id;
            Unit = unit;          
        }

        private AccountItem() { }

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

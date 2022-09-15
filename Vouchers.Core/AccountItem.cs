using System;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class AccountItem : Entity
    {
        public Guid HolderId { get; }
        public Account Holder { get; }

        public decimal Balance { get; protected set; }

        public Guid UnitId { get; }
        public Unit Unit { get; }

        public static AccountItem Create(Account holder, decimal balance, Unit unit) =>
            new AccountItem(Guid.NewGuid(), holder, balance, unit);

        internal AccountItem(Guid id, Account holder, decimal balance, Unit unit) : base(id)
        {
            HolderId = holder.Id;
            Holder = holder;
            
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

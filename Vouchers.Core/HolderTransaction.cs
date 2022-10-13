using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class HolderTransaction : Entity
    {
        public DateTime Timestamp { get; private set; }

        public Guid CreditorAccountId { get;}
        public Account CreditorAccount { get; }

        public Guid DebtorAccountId { get;}
        public Account DebtorAccount { get; }
        
        public UnitTypeQuantity Quantity { get; private set; }

        public ICollection<HolderTransactionItem> TransactionItems { get; }

        public string Message { get; }
        public bool IsPerformed { get; private set; }

        public static HolderTransaction Create(Account creditorAccount, Account debtorAccount, UnitType value, string message) =>
            new HolderTransaction(Guid.NewGuid(), DateTime.Now, creditorAccount, debtorAccount, value, message);

        internal HolderTransaction(Guid id, DateTime timestamp, Account creditorAccount, Account debtorAccount, UnitType unitType, string message) : base(id)
        {
            Timestamp = timestamp;

            CreditorAccountId = creditorAccount.Id;
            CreditorAccount = creditorAccount;

            DebtorAccountId = debtorAccount.Id;
            DebtorAccount = debtorAccount;            

            Quantity = UnitTypeQuantity.Create(0, unitType);

            Message = message;

            if (CreditorAccount.Equals(DebtorAccount))
                throw new CoreException("Creditor and debtor are the same");

            TransactionItems = new List<HolderTransactionItem>();            

        }
    
        private HolderTransaction(){ }

        public void AddTransactionItem(HolderTransactionItem item)
        {
            if (item.Quantity.Unit.UnitType.NotEquals(Quantity.UnitType))
                throw new CoreException("Transaction and item have different voucher values");

            if (item.Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException("Transaction contains expired vouchers");

            Quantity += item.Quantity;
            TransactionItems.Add(item);           
        }
        
        public void Perform() {
            if (Quantity.Amount == 0)
                throw new CoreException("Amount must be greater than 0");

            foreach (var item in TransactionItems) {
                item.CreditAccountItem.ProcessCredit(item.Quantity.Amount);
                item.DebitAccountItem.ProcessDebit(item.Quantity.Amount);
            }

            IsPerformed = true;
        }
    }
}

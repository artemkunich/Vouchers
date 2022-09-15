using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class HolderTransaction : Entity
    {
        public DateTime Timestamp { get; private set; }

        public Guid CreditorId { get;}
        public Account Creditor { get; }

        public Guid DebtorId { get;}
        public Account Debtor { get; }
        
        public UnitTypeQuantity Quantity { get; private set; }

        public ICollection<HolderTransactionItem> TransactionItems { get; }

        public bool IsPerformed { get; private set; }

        public static HolderTransaction Create(Account creditor, Account debtor, UnitType value) =>
            new HolderTransaction(Guid.NewGuid(), DateTime.Now, creditor, debtor, value);

        internal HolderTransaction(Guid id, DateTime timestamp, Account creditor, Account debtor, UnitType unitType) : base(id)
        {
            Timestamp = timestamp;

            CreditorId = creditor.Id;
            Creditor = creditor;

            DebtorId = debtor.Id;
            Debtor = debtor;            

            Quantity = UnitTypeQuantity.Create(0, unitType);

            if (Creditor.Equals(Debtor))
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

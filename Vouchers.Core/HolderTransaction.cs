using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class HolderTransaction
    {
        public Guid Id { get; }

        public DateTime Timestamp { get; private set; }

        public DomainAccount Creditor { get; }
        public DomainAccount Debtor { get; }
        
        public VoucherValueQuantity Quantity { get; private set; }

        public ICollection<HolderTransactionItem> TransactionItems { get; }

        public bool IsPerformed { get; private set; }

        public static HolderTransaction Create(DomainAccount creditor, DomainAccount debtor, VoucherValue value)
        {
            if (creditor.Domain.NotEquals(debtor.Domain))
                throw new ApplicationException("Creditor and debtor are not in the same domain");

            return new HolderTransaction(Guid.NewGuid(), DateTime.Now, creditor, debtor, value);
        }

        internal HolderTransaction(Guid id, DateTime timestamp, DomainAccount creditor, DomainAccount debtor, VoucherValue value) : this(timestamp, creditor, debtor, value) =>
            Id = id;

        internal HolderTransaction(DateTime timestamp, DomainAccount creditor, DomainAccount debtor, VoucherValue value)
        {
            Timestamp = timestamp;

            Creditor = creditor;
            Debtor = debtor;            
            
            Quantity = VoucherValueQuantity.Create(0, value);

            if (Creditor.Equals(Debtor))
                throw new CoreException("Creditor and debtor are the same");

            TransactionItems = new List<HolderTransactionItem>();            

        }
    
        private HolderTransaction(){ }

        public void AddTransactionItem(HolderTransactionItem item)
        {
            if (item.Quantity.Unit.Value.NotEquals(Quantity.Unit))
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
                item.CreditAccount.ProcessCredit(item.Quantity.Amount);
                item.DebitAccount.ProcessDebit(item.Quantity.Amount);
            }

            IsPerformed = true;
        }
    }
}

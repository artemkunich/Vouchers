using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;
using System.Globalization;

namespace Vouchers.Core
{
    public sealed class HolderTransaction : Entity<Guid>
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

        public static HolderTransaction Create(Account creditorAccount, Account debtorAccount, UnitType value, string message, CultureInfo cultureInfo = null) =>
            new HolderTransaction(Guid.NewGuid(), DateTime.Now, creditorAccount, debtorAccount, value, message, cultureInfo);

        private HolderTransaction(Guid id, DateTime timestamp, Account creditorAccount, Account debtorAccount, UnitType unitType, string message, CultureInfo cultureInfo = null) : base(id)
        {
            Timestamp = timestamp;

            CreditorAccountId = creditorAccount.Id;
            CreditorAccount = creditorAccount;

            DebtorAccountId = debtorAccount.Id;
            DebtorAccount = debtorAccount;            

            Quantity = UnitTypeQuantity.Create(0, unitType);

            Message = message;

            if (CreditorAccount.Equals(DebtorAccount))
                throw new CoreException("CreditorAndDebtorAreTheSame", cultureInfo);

            TransactionItems = new List<HolderTransactionItem>();            

        }
    
        private HolderTransaction(){ }

        public void AddTransactionItem(HolderTransactionItem item, CultureInfo cultureInfo = null)
        {
            if (item.Quantity.Unit.UnitType.NotEquals(Quantity.UnitType))
                throw new CoreException("TransactionAndItemHaveDifferentUnitTypes", cultureInfo);

            if (item.Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException("TransactionContainsExpiredUnits", cultureInfo);

            Quantity = Quantity.Add(item.Quantity);
            TransactionItems.Add(item);           
        }
        
        public void Perform(CultureInfo cultureInfo = null) {
            if (Quantity.Amount == 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);

            foreach (var item in TransactionItems) {
                item.CreditAccountItem.ProcessCredit(item.Quantity.Amount);
                item.DebitAccountItem.ProcessDebit(item.Quantity.Amount);
            }

            IsPerformed = true;
        }
    }
}

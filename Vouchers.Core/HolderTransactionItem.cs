using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class HolderTransactionItem : Entity
    {
        public UnitQuantity Quantity { get; }

        public Guid CreditAccountItemId { get; }
        public AccountItem CreditAccountItem { get; }

        public Guid DebitAccountItemId { get; }
        public AccountItem DebitAccountItem { get; }

        public static HolderTransactionItem Create(UnitQuantity quantity, AccountItem creditAccount, AccountItem debitAccount) =>
            new HolderTransactionItem(Guid.NewGuid(), quantity, creditAccount, debitAccount);

        internal HolderTransactionItem(Guid id, UnitQuantity quantity, AccountItem creditAccountItem, AccountItem debitAccountItem) : base(id)
        {
            if (creditAccountItem.Equals(debitAccountItem))
                throw new CoreException("Credit and debit accounts equal");

            if (creditAccountItem.Unit.NotEquals(quantity.Unit))
                throw new CoreException("Credit account and item have different units");

            if (debitAccountItem.Unit.NotEquals(quantity.Unit))
                throw new CoreException("Debit account and item have different units");

            if(!quantity.Unit.CanBeExchanged && creditAccountItem.Holder.NotEquals(quantity.Unit.UnitType.Issuer) && debitAccountItem.Holder.NotEquals(quantity.Unit.UnitType.Issuer))
                throw new CoreException("Item's unit cannot be exchanged");

            Quantity = quantity;

            CreditAccountItemId = creditAccountItem.Id;
            CreditAccountItem = creditAccountItem;

            DebitAccountItemId = debitAccountItem.Id;
            DebitAccountItem = debitAccountItem;

        }

        private HolderTransactionItem() { }

    }
}

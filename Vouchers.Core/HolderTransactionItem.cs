using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class HolderTransactionItem
    {        
        public Guid Id { get; }

        public VoucherQuantity Quantity { get; }

        public VoucherAccount CreditAccount { get; }

        public VoucherAccount DebitAccount { get; }

        public static HolderTransactionItem Create(VoucherQuantity quantity, VoucherAccount creditAccount, VoucherAccount debitAccount) =>
            new HolderTransactionItem(Guid.NewGuid(), quantity, creditAccount, debitAccount);

        internal HolderTransactionItem(Guid id, VoucherQuantity quantity, VoucherAccount creditAccount, VoucherAccount debitAccount) : this(quantity, creditAccount, debitAccount) =>
            Id = id;

        internal HolderTransactionItem(VoucherQuantity quantity, VoucherAccount creditAccount, VoucherAccount debitAccount)
        {
            if (creditAccount.Equals(debitAccount))
                throw new CoreException("Credit and debit accounts are the same");

            if (creditAccount.Unit.NotEquals(quantity.Unit))
                throw new CoreException("Credit account and item have different units");

            if (debitAccount.Unit.NotEquals(quantity.Unit))
                throw new CoreException("Debit account and item have different units");

            if(!quantity.Unit.CanBeExchanged && creditAccount.Holder.NotEquals(quantity.Unit.Value.Issuer) && debitAccount.Holder.NotEquals(quantity.Unit.Value.Issuer))
                throw new CoreException("Item's unit cannot be exchanged");

            Quantity = quantity;
            CreditAccount = creditAccount;
            DebitAccount = debitAccount;
        }

        private HolderTransactionItem() { }

    }
}

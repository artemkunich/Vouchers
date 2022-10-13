using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class HolderTransactionRequest : Entity
    {
        public DateTime DueDate { get; }

        public Guid? CreditorAccountId { get; }
        public Account CreditorAccount { get; }

        public Guid DebtorAccountId { get; }
        public Account DebtorAccount { get; }     

        public UnitTypeQuantity Quantity { get; }

        public TimeSpan? MaxDurationBeforeValidityStart { get; }
        public TimeSpan? MinDurationBeforeValidityEnd { get; }
        
        public bool MustBeExchangeable { get; }

        public string Message { get; }

        public Guid? TransactionId { get; private set; }
        public HolderTransaction Transaction { get; private set; }

        private HolderTransactionRequest() { }

        public HolderTransactionRequest (Guid id, DateTime dueDate, Account creditorAccount, Account debtorAccount, UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd, bool mustBeExchangeable, string message) : base(id)
        {
            if (quantity.Amount <= 0)
                throw new CoreException("Amount must be greater than 0");

            DueDate = dueDate;

            DebtorAccountId = debtorAccount.Id;
            DebtorAccount = debtorAccount;

            Quantity = quantity;

            if (quantity.UnitType.IssuerAccount.Equals(DebtorAccount))
            {
                if (maxDurationBeforeValidityStart != TimeSpan.Zero)
                    throw new CoreException("Issuer can not set maxDurationBeforeValidityStart");

                if (minDurationBeforeValidityEnd != TimeSpan.Zero)
                    throw new CoreException("Issuer can not set minDurationBeforeValidityEnd");

                if (mustBeExchangeable)
                    throw new CoreException("Issuer can not require exchangeability");
            }

            MaxDurationBeforeValidityStart = maxDurationBeforeValidityStart;
            MinDurationBeforeValidityEnd = minDurationBeforeValidityEnd;
            MustBeExchangeable = mustBeExchangeable;

            Message = message;

            if (creditorAccount is null)
                return;

            if (creditorAccount.Equals(debtorAccount))
                throw new CoreException("Creditor and debtor can not be the same");

            CreditorAccountId = creditorAccount.Id;
            CreditorAccount = creditorAccount;

            
        }

        public static HolderTransactionRequest Create(DateTime validTo, Account creditorAccount, Account debtorAccount, UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd, bool mustBeExchangeable, string message) =>
            new HolderTransactionRequest(Guid.NewGuid(), validTo, creditorAccount, debtorAccount, quantity, maxDurationBeforeValidityStart, minDurationBeforeValidityEnd, mustBeExchangeable, message);

        public void Perform(HolderTransaction transaction)
        {
            if(Transaction != null)
                throw new CoreException($"Transaction is already performed");

            if (CreditorAccount != null && CreditorAccount.NotEquals(transaction.CreditorAccount))
                throw new CoreException($"Request's creditor is not satisfied by transaction");

            if (DebtorAccount.NotEquals(transaction.DebtorAccount))
                throw new CoreException($"Request's debtor is not satisfied by transaction");

            if (Quantity.UnitType.NotEquals(transaction.Quantity.UnitType))
                throw new CoreException($"Request's unit is not satisfied by transaction");

            if (Quantity.Amount != transaction.Quantity.Amount)
                throw new CoreException($"Request's amount is not satisfied by transaction");

            foreach (var item in transaction.TransactionItems)
            {
                if (MaxDurationBeforeValidityStart is not null && item.Quantity.Unit.ValidFrom > DateTime.Now.Add(MaxDurationBeforeValidityStart.Value))
                    throw new CoreException($"Request's maxValidFrom is not satisfied by transaction");

                if (MinDurationBeforeValidityEnd is not null && item.Quantity.Unit.ValidTo < DateTime.Now.Add(MinDurationBeforeValidityEnd.Value))
                    throw new CoreException($"Request's minValidTo is not satisfied by transaction");

                if (MustBeExchangeable && !item.Quantity.Unit.CanBeExchanged)
                    throw new CoreException($"Request's mustBeExchangeable is not satisfied by transaction");
            }

            transaction.Perform();
            TransactionId = transaction.Id;
            Transaction = transaction;
        }

    }
}

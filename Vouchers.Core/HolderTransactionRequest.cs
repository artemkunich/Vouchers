using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;
using System.Globalization;

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

        private HolderTransactionRequest (Guid id, DateTime dueDate, Account creditorAccount, Account debtorAccount, UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd, bool mustBeExchangeable, string message, CultureInfo cultureInfo = null) : base(id)
        {
            if (quantity.Amount <= 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);

            DueDate = dueDate;

            DebtorAccountId = debtorAccount.Id;
            DebtorAccount = debtorAccount;

            Quantity = quantity;

            if (quantity.UnitType.IssuerAccount.Equals(DebtorAccount))
            {
                if (maxDurationBeforeValidityStart != TimeSpan.Zero)
                    throw new CoreException("IssuerCannotSetMaxDurationBeforeValidityStart", cultureInfo);

                if (minDurationBeforeValidityEnd != TimeSpan.Zero)
                    throw new CoreException("IssuerCannotSetMinDurationBeforeValidityEnd", cultureInfo);

                if (mustBeExchangeable)
                    throw new CoreException("IssuerCannotRequireExchangeability", cultureInfo);
            }

            MaxDurationBeforeValidityStart = maxDurationBeforeValidityStart;
            MinDurationBeforeValidityEnd = minDurationBeforeValidityEnd;
            MustBeExchangeable = mustBeExchangeable;

            Message = message;

            if (creditorAccount is null)
                return;

            if (creditorAccount.Equals(debtorAccount))
                throw new CoreException("CreditorAndDebtorAreTheSame", cultureInfo);

            CreditorAccountId = creditorAccount.Id;
            CreditorAccount = creditorAccount;

            
        }

        public static HolderTransactionRequest Create(DateTime validTo, Account creditorAccount, Account debtorAccount, UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd, bool mustBeExchangeable, string message, CultureInfo cultureInfo = null) =>
            new HolderTransactionRequest(Guid.NewGuid(), validTo, creditorAccount, debtorAccount, quantity, maxDurationBeforeValidityStart, minDurationBeforeValidityEnd, mustBeExchangeable, message, cultureInfo);

        public void Perform(HolderTransaction transaction, CultureInfo cultureInfo = null)
        {
            if(Transaction != null)
                throw new CoreException("TransactionIsAlreadyPerformed", cultureInfo);

            if (CreditorAccount != null && CreditorAccount.NotEquals(transaction.CreditorAccount))
                throw new CoreException("RequestCreditorIsNotSatisfiedByTransaction", cultureInfo);

            if (DebtorAccount.NotEquals(transaction.DebtorAccount))
                throw new CoreException("RequestDebtorIsNotSatisfiedByTransaction", cultureInfo);

            if (Quantity.UnitType.NotEquals(transaction.Quantity.UnitType))
                throw new CoreException("RequestUnitIsNotSatisfiedByTransaction", cultureInfo);

            if (Quantity.Amount != transaction.Quantity.Amount)
                throw new CoreException("RequestAmountIsNotSatisfiedByTransaction", cultureInfo);

            foreach (var item in transaction.TransactionItems)
            {
                if (MaxDurationBeforeValidityStart is not null && item.Quantity.Unit.ValidFrom > DateTime.Now.Add(MaxDurationBeforeValidityStart.Value))
                    throw new CoreException("RequestMaxValidFromIsNotSatisfiedByTransaction", cultureInfo);

                if (MinDurationBeforeValidityEnd is not null && item.Quantity.Unit.ValidTo < DateTime.Now.Add(MinDurationBeforeValidityEnd.Value))
                    throw new CoreException("RequestMinValidToIsNotSatisfiedByTransaction", cultureInfo);

                if (MustBeExchangeable && !item.Quantity.Unit.CanBeExchanged)
                    throw new CoreException("RequestMustBeExchangeableIsNotSatisfiedByTransaction", cultureInfo);
            }

            transaction.Perform(cultureInfo);
            TransactionId = transaction.Id;
            Transaction = transaction;
        }

    }
}

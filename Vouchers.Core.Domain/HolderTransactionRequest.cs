using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionRequest : AggregateRoot<Guid>
{
    public DateTime DueDate { get; init; }

    public Guid? CreditorAccountId { get; init; }
    public Account CreditorAccount { get; init; }

    public Guid DebtorAccountId { get; init; }
    public Account DebtorAccount { get; init; }     

    public UnitTypeQuantity Quantity { get; init; }

    public TimeSpan? MaxDurationBeforeValidityStart { get; init; }
    public TimeSpan? MinDurationBeforeValidityEnd { get; init; }
    
    public bool MustBeExchangeable { get; init; }

    public string Message { get; init; }

    public Guid? TransactionId { get; private set; }
    public HolderTransaction Transaction { get; private set; }
    

    public static HolderTransactionRequest Create(Guid id, DateTime dueDate, Account creditorAccount, Account debtorAccount,
        UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd,
        bool mustBeExchangeable, string message, CultureInfo cultureInfo = null)
    {
        if (quantity.Amount <= 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);
        
        if (quantity.UnitType.IssuerAccount.Equals(debtorAccount))
        {
            if (maxDurationBeforeValidityStart != TimeSpan.Zero)
                throw new CoreException("IssuerCannotSetMaxDurationBeforeValidityStart", cultureInfo);

            if (minDurationBeforeValidityEnd != TimeSpan.Zero)
                throw new CoreException("IssuerCannotSetMinDurationBeforeValidityEnd", cultureInfo);

            if (mustBeExchangeable)
                throw new CoreException("IssuerCannotRequireExchangeability", cultureInfo);
        }
        
        if (creditorAccount is not null && debtorAccount.Equals(creditorAccount))
            throw new CoreException("CreditorAndDebtorAreTheSame", cultureInfo);

        return new()
        {
            Id = id,
            DueDate = dueDate,

            DebtorAccountId = debtorAccount.Id,
            DebtorAccount = debtorAccount,

            Quantity = quantity,
            
            MaxDurationBeforeValidityStart = maxDurationBeforeValidityStart,
            MinDurationBeforeValidityEnd = minDurationBeforeValidityEnd,
            MustBeExchangeable = mustBeExchangeable,

            Message = message,
            
            CreditorAccount = creditorAccount,
            CreditorAccountId = creditorAccount?.Id
        };

    }

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


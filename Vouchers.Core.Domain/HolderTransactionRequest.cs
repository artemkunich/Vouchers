using System;
using Vouchers.Primitives;

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
        bool mustBeExchangeable, string message)
    {
        if (quantity.Amount <= 0)
            throw CoreException.AmountIsNotPositive;
        
        if (quantity.UnitType.IssuerAccount.Equals(debtorAccount))
        {
            if (maxDurationBeforeValidityStart != TimeSpan.Zero)
                throw CoreException.IssuerCannotSetMaxDurationBeforeValidityStart;

            if (minDurationBeforeValidityEnd != TimeSpan.Zero)
                throw CoreException.IssuerCannotSetMinDurationBeforeValidityEnd;

            if (mustBeExchangeable)
                throw CoreException.IssuerCannotRequireExchangeability;
        }
        
        if (creditorAccount is not null && debtorAccount.Equals(creditorAccount))
            throw CoreException.CreditorAndDebtorAccountsAreTheSame;

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

    public void Perform(HolderTransaction transaction)
    {
        if(Transaction != null)
            throw CoreException.TransactionRequestIsAlreadyPerformed;

        if (CreditorAccount != null && CreditorAccount.NotEquals(transaction.CreditorAccount))
            throw CoreException.RequestCreditorIsNotSatisfiedByTransaction;

        if (DebtorAccount.NotEquals(transaction.DebtorAccount))
            throw CoreException.RequestDebtorIsNotSatisfiedByTransaction;

        if (Quantity.UnitType.NotEquals(transaction.Quantity.UnitType))
            throw CoreException.RequestUnitIsNotSatisfiedByTransaction;

        if (Quantity.Amount != transaction.Quantity.Amount)
            throw CoreException.RequestAmountIsNotSatisfiedByTransaction;

        foreach (var item in transaction.TransactionItems)
        {
            if (MaxDurationBeforeValidityStart is not null && item.Unit.ValidFrom > DateTime.Now.Add(MaxDurationBeforeValidityStart.Value))
                throw CoreException.RequestMaxValidFromIsNotSatisfiedByTransaction;

            if (MinDurationBeforeValidityEnd is not null && item.Unit.ValidTo < DateTime.Now.Add(MinDurationBeforeValidityEnd.Value))
                throw CoreException.RequestMinValidToIsNotSatisfiedByTransaction;

            if (MustBeExchangeable && !item.Unit.CanBeExchanged)
                throw CoreException.RequestMustBeExchangeableIsNotSatisfiedByTransaction;
        }

        transaction.Perform();
        TransactionId = transaction.Id;
        Transaction = transaction;
    }

}


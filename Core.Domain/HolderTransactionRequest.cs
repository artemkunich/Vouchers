using System;
using Vouchers.Core.Domain.Exceptions;
using Akunich.Domain.Abstractions;

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
            throw new NotPositiveAmountException();
        
        if (quantity.UnitType.IssuerAccount.Equals(debtorAccount))
        {
            if (maxDurationBeforeValidityStart != TimeSpan.Zero)
                throw new IssuerCannotSetMaxDurationBeforeValidityStartException();

            if (minDurationBeforeValidityEnd != TimeSpan.Zero)
                throw new IssuerCannotSetMinDurationBeforeValidityEndException();

            if (mustBeExchangeable)
                throw new IssuerCannotRequireExchangeabilityException();
        }
        
        if (creditorAccount is not null && debtorAccount.Equals(creditorAccount))
            throw new CreditorAndDebtorAccountsAreEqualException();

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
            throw new TransactionRequestIsAlreadyPerformedException();

        if (CreditorAccount != null && CreditorAccount.NotEquals(transaction.CreditorAccount))
            throw new RequestCreditorIsNotSatisfiedByTransactionException();

        if (DebtorAccount.NotEquals(transaction.DebtorAccount))
            throw new RequestDebtorIsNotSatisfiedByTransactionException();

        if (Quantity.UnitType.NotEquals(transaction.Quantity.UnitType))
            throw new RequestUnitIsNotSatisfiedByTransactionException();

        if (Quantity.Amount != transaction.Quantity.Amount)
            throw new RequestAmountIsNotSatisfiedByTransactionException();

        foreach (var item in transaction.TransactionItems)
        {
            if (MaxDurationBeforeValidityStart is not null && item.Unit.ValidFrom > DateTime.Now.Add(MaxDurationBeforeValidityStart.Value))
                throw new RequestMaxValidFromIsNotSatisfiedByTransactionException();

            if (MinDurationBeforeValidityEnd is not null && item.Unit.ValidTo < DateTime.Now.Add(MinDurationBeforeValidityEnd.Value))
                throw new RequestMinValidToIsNotSatisfiedByTransactionException();

            if (MustBeExchangeable && !item.Unit.CanBeExchanged)
                throw new RequestMustBeExchangeableIsNotSatisfiedByTransactionException();
        }

        transaction.Perform();
        TransactionId = transaction.Id;
        Transaction = transaction;
    }

}


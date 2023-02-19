using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class HolderTransaction : AggregateRoot<Guid>
{
    public DateTime Timestamp { get; private set; }
    public Guid CreditorAccountId { get; init; }
    public Account CreditorAccount { get; init; }
    public Guid DebtorAccountId { get; init; }
    public Account DebtorAccount { get; init; }
    public UnitTypeQuantity Quantity { get; private set; }
    public IEnumerable<HolderTransactionItem> TransactionItems => _transactionItems;
    private ICollection<HolderTransactionItem> _transactionItems;

    public string Message { get; init; }
    public bool IsPerformed { get; private set; }

    public static HolderTransaction Create(Guid id, DateTime currentDateTime, Account creditorAccount, Account debtorAccount, UnitType unitType, string message)
    {
        if (creditorAccount.Equals(debtorAccount))
            throw CoreException.CreditorAndDebtorAccountsAreTheSame;

        return new()
        {
            Id = id,
            Timestamp = currentDateTime,
            CreditorAccountId = creditorAccount.Id,
            CreditorAccount = creditorAccount,
            DebtorAccountId = debtorAccount.Id,
            DebtorAccount = debtorAccount,
            Quantity = UnitTypeQuantity.Create(0, unitType),
            Message = message,
            _transactionItems = new List<HolderTransactionItem>()
        };
    }

    public void AddTransactionItem(HolderTransactionItem item)
    {
        if (item.HolderTransactionId != Id)
            throw CoreException.ItemDoesNotBelongToTransaction;
        
        if (item.Unit.ValidTo < Timestamp)
            throw CoreException.TransactionContainsExpiredUnits;

        if(TransactionItems.Any(i => i.Id == item.Id))
            throw CoreException.TransactionAlreadyContainsItem;
        
        var unitQuantity = UnitQuantity.Create(item.Amount, item.Unit);
        Quantity = Quantity.Add(unitQuantity);
        _transactionItems.Add(item);           
    }
    
    public void Perform() 
    {
        if (IsPerformed)
            throw CoreException.TransactionIsAlreadyPerformed;;
            
        if (Quantity.Amount == 0)
            throw CoreException.AmountIsNotPositive;
        
        foreach (var item in TransactionItems) 
        {
            item.CreditAccountItem.ProcessCredit(item.Amount);
            item.DebitAccountItem.ProcessDebit(item.Amount);
        }

        IsPerformed = true;
    }
}

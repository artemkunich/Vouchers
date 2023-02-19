using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionItem : Entity<Guid>
{
    public Guid HolderTransactionId { get; init; }
    public HolderTransaction HolderTransaction { get; init; }
    public decimal Amount { get; init; }
    public Unit Unit => DebitAccountItem.Unit;
    
    public Guid CreditAccountItemId { get; init; }
    public AccountItem CreditAccountItem { get; init; }

    public Guid DebitAccountItemId { get; init; }
    public AccountItem DebitAccountItem { get; init; }

    public static HolderTransactionItem Create(Guid id, decimal amount, AccountItem creditAccountItem, AccountItem debitAccountItem, HolderTransaction holderTransaction)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;
        
        if (creditAccountItem.Equals(debitAccountItem))
            throw CoreException.CreditorAndDebtorAccountsAreTheSame;

        if (creditAccountItem.Unit.NotEquals(debitAccountItem.Unit))
            throw CoreException.CreditAccountAndDebitAccountHaveDifferentUnits;

        var unit = creditAccountItem.Unit;

        if(!unit.CanBeExchanged && creditAccountItem.HolderAccount.NotEquals(unit.UnitType.IssuerAccount) && debitAccountItem.HolderAccount.NotEquals(unit.UnitType.IssuerAccount))
            throw CoreException.ItemUnitCannotBeExchanged;

        var newHolderTransactionItem = new HolderTransactionItem
        {
            Id = id,
            Amount = amount,

            CreditAccountItemId = creditAccountItem.Id,
            CreditAccountItem = creditAccountItem,

            DebitAccountItemId = debitAccountItem.Id,
            DebitAccountItem = debitAccountItem,
            
            HolderTransactionId = holderTransaction.Id,
            HolderTransaction = holderTransaction
        };
        
        holderTransaction.AddTransactionItem(newHolderTransactionItem);

        return newHolderTransactionItem;
    }
}

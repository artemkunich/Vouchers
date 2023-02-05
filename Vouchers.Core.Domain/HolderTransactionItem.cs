using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionItem : Entity<Guid>
{
    public UnitQuantity Quantity { get; init; }

    public Guid CreditAccountItemId { get; init; }
    public AccountItem CreditAccountItem { get; init; }

    public Guid DebitAccountItemId { get; init; }
    public AccountItem DebitAccountItem { get; init; }

    public static HolderTransactionItem Create(Guid id, UnitQuantity quantity, AccountItem creditAccountItem, AccountItem debitAccountItem)
    {
        if (creditAccountItem.Equals(debitAccountItem))
            throw CoreException.CreditorAndDebtorAccountsAreTheSame;

        if (creditAccountItem.Unit.NotEquals(quantity.Unit))
            throw CoreException.CreditAccountAndItemHaveDifferentUnits;

        if (debitAccountItem.Unit.NotEquals(quantity.Unit))
            throw CoreException.DebitAccountAndItemHaveDifferentUnits;

        if(!quantity.Unit.CanBeExchanged && creditAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount) && debitAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount))
            throw CoreException.ItemUnitCannotBeExchanged;

        return new()
        {
            Id = id,
            Quantity = quantity,

            CreditAccountItemId = creditAccountItem.Id,
            CreditAccountItem = creditAccountItem,

            DebitAccountItemId = debitAccountItem.Id,
            DebitAccountItem = debitAccountItem,
        };
    }

}

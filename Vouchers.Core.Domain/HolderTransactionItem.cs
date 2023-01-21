using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionItem : Entity<Guid>
{
    public UnitQuantity Quantity { get; init; }

    public Guid CreditAccountItemId { get; init; }
    public AccountItem CreditAccountItem { get; init; }

    public Guid DebitAccountItemId { get; init; }
    public AccountItem DebitAccountItem { get; init; }

    public static HolderTransactionItem Create(Guid id, UnitQuantity quantity, AccountItem creditAccountItem, AccountItem debitAccountItem, CultureInfo cultureInfo = null)
    {
        if (creditAccountItem.Equals(debitAccountItem))
            throw new CoreException("CreditAndDebitAccountsAreEqual", cultureInfo);

        if (creditAccountItem.Unit.NotEquals(quantity.Unit))
            throw new CoreException("CreditAccountAndItemHaveDifferentUnits", cultureInfo);

        if (debitAccountItem.Unit.NotEquals(quantity.Unit))
            throw new CoreException("DebitAccountAndItemHaveDifferentUnits", cultureInfo);

        if(!quantity.Unit.CanBeExchanged && creditAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount) && debitAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount))
            throw new CoreException("ItemUnitCannotBeExchanged", cultureInfo);

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

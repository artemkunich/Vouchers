using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionItem : AggregateRoot<Guid>
{
    public UnitQuantity Quantity { get; }

    public Guid CreditAccountItemId { get; }
    public AccountItem CreditAccountItem { get; }

    public Guid DebitAccountItemId { get; }
    public AccountItem DebitAccountItem { get; }

    public static HolderTransactionItem Create(UnitQuantity quantity, AccountItem creditAccount, AccountItem debitAccount, CultureInfo cultureInfo = null) =>
        new HolderTransactionItem(Guid.NewGuid(), quantity, creditAccount, debitAccount, cultureInfo);

    private HolderTransactionItem(Guid id, UnitQuantity quantity, AccountItem creditAccountItem, AccountItem debitAccountItem, CultureInfo cultureInfo = null) : base(id)
    {
        if (creditAccountItem.Equals(debitAccountItem))
            throw new CoreException("CreditAndDebitAccountsAreEqual", cultureInfo);

        if (creditAccountItem.Unit.NotEquals(quantity.Unit))
            throw new CoreException("CreditAccountAndItemHaveDifferentUnits", cultureInfo);

        if (debitAccountItem.Unit.NotEquals(quantity.Unit))
            throw new CoreException("DebitAccountAndItemHaveDifferentUnits", cultureInfo);

        if(!quantity.Unit.CanBeExchanged && creditAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount) && debitAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount))
            throw new CoreException("ItemUnitCannotBeExchanged", cultureInfo);

        Quantity = quantity;

        CreditAccountItemId = creditAccountItem.Id;
        CreditAccountItem = creditAccountItem;

        DebitAccountItemId = debitAccountItem.Id;
        DebitAccountItem = debitAccountItem;

    }

    private HolderTransactionItem() { }

}

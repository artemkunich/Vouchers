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

    public static Result<HolderTransactionItem> Create(Guid id, UnitQuantity quantity, AccountItem creditAccountItem, AccountItem debitAccountItem, CultureInfo cultureInfo = null) =>
        Result.Create()
            .IfTrueAddError(creditAccountItem.Equals(debitAccountItem), Errors.CreditorAndDebtorAreTheSame(cultureInfo))
            .IfTrueAddError(creditAccountItem.Unit.NotEquals(quantity.Unit),
                Errors.CreditAccountAndItemHaveDifferentUnits(cultureInfo))
            .IfTrueAddError(debitAccountItem.Unit.NotEquals(quantity.Unit),
                Errors.DebitAccountAndItemHaveDifferentUnits(cultureInfo))
            .IfTrueAddError(
                !quantity.Unit.CanBeExchanged &&
                creditAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount) &&
                debitAccountItem.HolderAccount.NotEquals(quantity.Unit.UnitType.IssuerAccount),
                Errors.ItemUnitCannotBeExchanged(cultureInfo))
            .SetValue(new HolderTransactionItem
            {
                Id = id,
                Quantity = quantity,
                CreditAccountItemId = creditAccountItem.Id,
                CreditAccountItem = creditAccountItem,
                DebitAccountItemId = debitAccountItem.Id,
                DebitAccountItem = debitAccountItem,
            }); 
        
}

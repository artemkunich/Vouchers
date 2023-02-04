using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class IssuerTransaction : AggregateRoot<Guid>
{
    public DateTime Timestamp { get; init; }

    public UnitQuantity Quantity { get; init; }

    public Guid IssuerAccountItemId { get; init; }
    public AccountItem IssuerAccountItem { get; init; }

    public static Result<IssuerTransaction> Create(Guid id, DateTime timestamp, AccountItem issuerAccountItem,
        decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create()
            .IfTrueAddError(issuerAccountItem.Unit.ValidTo < DateTime.Today, Errors.UnitIsExpired(cultureInfo))
            .IfTrueAddError(issuerAccountItem.Unit.UnitType.IssuerAccount.NotEquals(issuerAccountItem.HolderAccount),
                Errors.AccountHolderAndUnitTypeIssuerAreDifferent(cultureInfo))
            .IfTrueAddError(issuerAccountItem.Unit.NotEquals(issuerAccountItem.Unit),
                Errors.AccountItemUnitAndTransactionUnitAreDifferent(cultureInfo))
            .IfTrueAddError(amount == 0, Errors.AmountIsNotPositive(cultureInfo))
            .SetValue(UnitQuantity.Create(amount, issuerAccountItem.Unit))
            .Map(quantity => new IssuerTransaction
            {
                Id = id,
                Timestamp = timestamp,
                Quantity = quantity.Value,
                IssuerAccountItemId = issuerAccountItem.Id,
                IssuerAccountItem = issuerAccountItem,
            });

    private IssuerTransaction()
    {
        //Empty
    }
    
    public Result<IssuerTransaction> Perform() =>
        Result.Create(this)
            .MergeResultErrors(transaction =>
            {
                var amount = transaction.Quantity.Amount;
                return amount > 0
                    ? transaction.IssuerAccountItem.ProcessDebit(amount)
                    : transaction.IssuerAccountItem.ProcessCredit(-amount)
                .MergeResultErrors(item => amount > 0
                    ? item.Unit.IncreaseSupply(amount)
                    : item.Unit.ReduceSupply(-amount)
                );
            });
}

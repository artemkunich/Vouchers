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
            .AddErrorIf(issuerAccountItem.Unit.ValidTo < DateTime.Today, Errors.UnitIsExpired(cultureInfo))
            .AddErrorIf(issuerAccountItem.Unit.UnitType.IssuerAccount.NotEquals(issuerAccountItem.HolderAccount),
                Errors.AccountHolderAndUnitTypeIssuerAreDifferent(cultureInfo))
            .AddErrorIf(issuerAccountItem.Unit.NotEquals(issuerAccountItem.Unit),
                Errors.AccountItemUnitAndTransactionUnitAreDifferent(cultureInfo))
            .AddErrorIf(amount == 0, Errors.AmountIsNotPositive(cultureInfo))
            .Map(() => UnitQuantity.Create(amount, issuerAccountItem.Unit))
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
            .IfSuccess(transaction =>
                {
                    var result = transaction.Quantity.Amount > 0
                        ? transaction.IssuerAccountItem.ProcessDebit(transaction.Quantity.Amount)
                        : transaction.IssuerAccountItem.ProcessCredit(-transaction.Quantity.Amount);

                    return result.IfSuccess(item => transaction.Quantity.Amount > 0
                        ? item.Unit.IncreaseSupply(Quantity.Amount)
                        : item.Unit.ReduceSupply(-Quantity.Amount)
                    );
                }
            );
}

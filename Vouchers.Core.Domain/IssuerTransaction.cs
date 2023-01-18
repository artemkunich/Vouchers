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

    public static IssuerTransaction Create(Guid id, DateTime timestamp, AccountItem issuerAccountItem, decimal amount, CultureInfo cultureInfo = null)
    {
        var quantity = UnitQuantity.Create(amount, issuerAccountItem.Unit);
        
        if (quantity.Unit.ValidTo < DateTime.Today)
            throw new CoreException("UnitIsExpired", cultureInfo);

        if (quantity.Unit.UnitType.IssuerAccount.NotEquals(issuerAccountItem.HolderAccount))
            throw new CoreException("AccountHolderAndUnitTypeIssuerAreDifferent", cultureInfo);

        if (quantity.Unit.NotEquals(issuerAccountItem.Unit))
            throw new CoreException("AccountItemUnitAndTransactionUnitAreDifferent", cultureInfo);

        if (quantity.Amount == 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);

        return new()
        {
            Id = id,
            Timestamp = timestamp,

            Quantity = quantity,
            
            IssuerAccountItemId = issuerAccountItem.Id,
            IssuerAccountItem = issuerAccountItem,
        };
    }

    public void Perform()
    {
        if (Quantity.Amount > 0)
        {
            IssuerAccountItem.ProcessDebit(Quantity.Amount);
            IssuerAccountItem.Unit.IncreaseSupply(Quantity.Amount);
        }
        else
        {
            IssuerAccountItem.ProcessCredit(-Quantity.Amount);
            IssuerAccountItem.Unit.ReduceSupply(-Quantity.Amount);
        }
    }
}
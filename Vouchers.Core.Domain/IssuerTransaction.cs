using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class IssuerTransaction : AggregateRoot<Guid>
{
    public DateTime Timestamp { get; init; }

    public UnitQuantity Quantity { get; init; }

    public Guid IssuerAccountItemId { get; init; }
    public AccountItem IssuerAccountItem { get; init; }

    public static IssuerTransaction Create(Guid id, DateTime timestamp, AccountItem issuerAccountItem, decimal amount)
    {
        var quantity = UnitQuantity.Create(amount, issuerAccountItem.Unit);
        
        if (quantity.Unit.ValidTo < DateTime.Today)
            throw CoreException.UnitIsExpired;

        if (quantity.Unit.UnitType.IssuerAccount.NotEquals(issuerAccountItem.HolderAccount))
            throw CoreException.AccountHolderAndUnitTypeIssuerAreDifferent;

        if (quantity.Unit.NotEquals(issuerAccountItem.Unit))
            throw CoreException.AccountItemUnitAndTransactionUnitAreDifferent;

        if (quantity.Amount == 0)
            throw CoreException.AmountIsNotPositive;

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
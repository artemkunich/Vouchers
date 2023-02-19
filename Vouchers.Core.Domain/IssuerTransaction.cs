using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class IssuerTransaction : AggregateRoot<Guid>
{
    public DateTime Timestamp { get; init; }
    public decimal Amount { get; init; }
    public Guid IssuerAccountItemId { get; init; }
    public AccountItem IssuerAccountItem { get; init; }

    public static IssuerTransaction Create(Guid id, DateTime currentDateTime, AccountItem issuerAccountItem, decimal amount)
    {
        if (amount == 0)
            throw CoreException.AmountIsZero;
        
        if (issuerAccountItem.Unit.ValidTo < currentDateTime)
            throw CoreException.UnitIsExpired;
        
        if (issuerAccountItem.Unit.UnitType.IssuerAccount.NotEquals(issuerAccountItem.HolderAccount))
            throw CoreException.AccountHolderAndUnitTypeIssuerAreDifferent;
        
        return new()
        {
            Id = id,
            Timestamp = currentDateTime,

            Amount = amount,
            
            IssuerAccountItemId = issuerAccountItem.Id,
            IssuerAccountItem = issuerAccountItem,
        };
    }

    public void Perform()
    {
        if (Amount > 0)
        {
            IssuerAccountItem.ProcessDebit(Amount);
            IssuerAccountItem.Unit.IncreaseSupply(Amount);
        }
        else
        {
            IssuerAccountItem.ProcessCredit(-Amount);
            IssuerAccountItem.Unit.ReduceSupply(-Amount);
        }
    }
}
using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class Account : AggregateRoot<Guid>
{
    public DateTime CreatedDateTime { get; init; }
    public decimal Supply { get; private set; }

    public static Account Create(Guid id, DateTime createdDateTime) => new ()
    {
        Id = id,
        CreatedDateTime = createdDateTime
    };

    public void IncreaseSupply(decimal amount) 
    { 
        if(amount <= 0)
            throw CoreException.AmountIsNotPositive;

        Supply += amount;
    }

    public void ReduceSupply(decimal amount)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;

        if (Supply < amount)
            throw CoreException.AmountIsGreaterThanSupply;

        Supply -= amount;
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}


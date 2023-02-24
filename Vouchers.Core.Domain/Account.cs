using System;
using Vouchers.Core.Domain.Exceptions;
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
            throw new NotPositiveAmountException();

        Supply += amount;
    }

    public void ReduceSupply(decimal amount)
    {
        if (amount <= 0)
            throw new NotPositiveAmountException();

        if (Supply < amount)
            throw new AmountIsGreaterThanSupplyException();

        Supply -= amount;
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}


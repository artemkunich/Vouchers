using System;
using Vouchers.Primitives;
using System.Globalization;

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

    public void IncreaseSupply(decimal amount, CultureInfo cultureInfo = null) 
    { 
        if(amount <= 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);

        Supply += amount;
    }

    public void ReduceSupply(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount <= 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);

        if (Supply < amount)
            throw new CoreException("AmountIsGreaterThanSupply", cultureInfo);

        Supply -= amount;
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}


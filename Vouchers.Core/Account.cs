using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;
using System.Globalization;

namespace Vouchers.Core;

[AggregateRoot]
public sealed class Account : Entity<Guid>
{
    public DateTime CreatedDateTime { get; }
    public decimal Supply { get; private set; }

    public static Account Create() =>
        new Account(Guid.NewGuid(), DateTime.Now);

    private Account(Guid id, DateTime createdDateTime) : base(id)
    {
        CreatedDateTime = createdDateTime;
    }   

    private Account() { }

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


using System;
using System.Globalization;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class UnitType : AggregateRoot<Guid>
{
    public Guid IssuerAccountId { get; init; }
    public Account IssuerAccount { get; init; }
    public decimal Supply { get; private set; }

    public static UnitType Create(Guid id, Account issuerAccount) => new()
    {
        Id = id,
        IssuerAccountId = issuerAccount.Id,
        IssuerAccount = issuerAccount,
    };

    public void IncreaseSupply(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount <= 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);
        Supply += amount;

        IssuerAccount.IncreaseSupply(amount);
    }

    public void ReduceSupply(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount <= 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);
        if (Supply < amount)
            throw new CoreException("AmountIsGreaterThanSupply", cultureInfo);
        Supply -= amount;

        IssuerAccount.ReduceSupply(amount);
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}


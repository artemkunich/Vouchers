using System;
using Vouchers.Core.Domain.Exceptions;
using Akunich.Domain.Abstractions;

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

    public void IncreaseSupply(decimal amount)
    {
        if (amount <= 0)
            throw new NotPositiveAmountException();
        Supply += amount;

        IssuerAccount.IncreaseSupply(amount);
    }

    public void ReduceSupply(decimal amount)
    {
        if (amount <= 0)
            throw new NotPositiveAmountException();
        if (Supply < amount)
            throw new AmountIsGreaterThanSupplyException();
        Supply -= amount;

        IssuerAccount.ReduceSupply(amount);
    }

    public bool CanBeRemoved()
    {
        return Supply == 0;
    }
}


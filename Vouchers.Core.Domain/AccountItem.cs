using System;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class AccountItem : Entity<Guid>
{
    public Guid HolderAccountId { get; init; }
    public Account HolderAccount { get; init; }

    public decimal Balance { get; private set; }

    public Guid UnitId { get; init; }
    public Unit Unit { get; init; }

    public static AccountItem Create(Guid id, Account holderAccount, Unit unit) => new()
    {
        Id = id,
        HolderAccountId = holderAccount.Id,
        HolderAccount = holderAccount,

        UnitId = unit.Id,
        Unit = unit
    };

    /// <summary>
    /// Increase balance by amount
    /// </summary>
    /// <param name="amount"></param>
    /// <exception cref="CoreException"></exception>
    public void ProcessDebit(decimal amount)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;
        
        Balance += amount;
    }
    
    /// <summary>
    /// Reduce balance by amount
    /// </summary>
    /// <param name="amount"></param>
    /// <exception cref="CoreException"></exception>
    public void ProcessCredit(decimal amount)
    {
        if (amount <= 0)
            throw CoreException.AmountIsNotPositive;
        
        if (amount > Balance)
            throw CoreException.AmountIsGreaterThanBalance;
        
        Balance -= amount;
    }
}


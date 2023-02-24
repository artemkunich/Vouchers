using System;
using Vouchers.Core.Domain.Exceptions;
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
            throw new NotPositiveAmountException();
        
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
            throw new NotPositiveAmountException();
        
        if (amount > Balance)
            throw new AmountIsGreaterThanBalanceException();
        
        Balance -= amount;
    }
}


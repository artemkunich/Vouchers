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

    public static AccountItem Create(Guid id, Account holderAccount, decimal balance, Unit unit) => new()
    {
        Id = id,
        HolderAccountId = holderAccount.Id,
        HolderAccount = holderAccount,
        
        Balance = balance,
        
        UnitId = unit.Id,
        Unit = unit
    };

    public void ProcessDebit(decimal amount)
    {
        Balance += amount;
    }
    public void ProcessCredit(decimal amount)
    {
        if (amount > Balance)
            throw CoreException.AmountIsGreaterThanBalance;
        
        Balance -= amount;
    }
}


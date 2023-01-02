using System;
using Vouchers.Entities;
using System.Globalization;

namespace Vouchers.Core;

[AggregateRoot]
public sealed class AccountItem : Entity<Guid>
{
    public Guid HolderAccountId { get; }
    public Account HolderAccount { get; }

    public decimal Balance { get; private set; }

    public Guid UnitId { get; }
    public Unit Unit { get; }

    public static AccountItem Create(Account holderAccount, decimal balance, Unit unit) =>
        new AccountItem(Guid.NewGuid(), holderAccount, balance, unit);

    private AccountItem(Guid id, Account holderAccount, decimal balance, Unit unit) : base(id)
    {
        HolderAccountId = holderAccount.Id;
        HolderAccount = holderAccount;
        
        Balance = balance;

        UnitId = unit.Id;
        Unit = unit;          
    }

    private AccountItem() { }

    public void ProcessDebit(decimal amount)
    {
        Balance += amount;
    }
    public void ProcessCredit(decimal amount, CultureInfo cultureInfo = null)
    {
        if (amount > Balance)
        {
            throw new CoreException("AmountIsGreaterThanBalance", cultureInfo);
        }
        Balance -= amount;
    }
}


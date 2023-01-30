using System;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class AccountItem : Entity<Guid>
{
    public Guid HolderAccountId { get; init; }
    public Account HolderAccount { get; init; }

    public decimal Balance { get; private set; }

    public Guid UnitId { get; init; }
    public Unit Unit { get; init; }

    public static Result<AccountItem> Create(Guid id, Account holderAccount, decimal balance, Unit unit) => 
        new AccountItem
        {
            Id = id,
            HolderAccountId = holderAccount.Id,
            HolderAccount = holderAccount,
            
            Balance = balance,
            
            UnitId = unit.Id,
            Unit = unit
        };

    private AccountItem()
    {
        //Empty
    }
    
    public Result<AccountItem> ProcessDebit(decimal amount) =>
        Result.Create(this)
            .IfSuccess(item => item.Balance += amount);

    public Result<AccountItem> ProcessCredit(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .AddErrorIf(amount > Balance, Errors.AmountIsGreaterThanBalance(cultureInfo))
            .IfSuccess(item => item.Balance -= amount);
}


using System;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class Account : AggregateRoot<Guid>
{
    public DateTime CreatedDateTime { get; init; }
    public decimal Supply { get; private set; }
    
    public static Result<Account> Create(Guid id, DateTime createdDateTime) => 
        new Account
        {
            Id = id,
            CreatedDateTime = createdDateTime
        };

    private Account()
    {
        //Empty
    }
    
    public Result<Account> IncreaseSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .Process(account => account.Supply += amount);

    public Result<Account> ReduceSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .IfTrueAddError(amount > Supply, Errors.AmountIsGreaterThanSupply(cultureInfo))
            .Process(account => account.Supply -= amount);

    public bool CanBeRemoved() => Supply == 0;
}


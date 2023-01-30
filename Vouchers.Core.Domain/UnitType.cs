using System;
using System.Globalization;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public sealed class UnitType : AggregateRoot<Guid>
{
    public Guid IssuerAccountId { get; init; }
    public Account IssuerAccount { get; init; }
    public decimal Supply { get; private set; }

    public static Result<UnitType> Create(Guid id, Account issuerAccount) => new UnitType()
    {
        Id = id,
        IssuerAccountId = issuerAccount.Id,
        IssuerAccount = issuerAccount,
    };

    private UnitType()
    {
        //Empty
    }
    
    public Result<UnitType> IncreaseSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .AddErrorIf(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .IfSuccess(unitType => unitType.Supply += amount)
            .IfSuccess(unitType => unitType.IssuerAccount.IncreaseSupply(amount));

    public Result<UnitType> ReduceSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .AddErrorIf(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .AddErrorIf(Supply < amount, Errors.AmountIsGreaterThanSupply(cultureInfo))
            .IfSuccess(unitType => unitType.Supply -= amount)
            .IfSuccess(unitType => unitType.IssuerAccount.ReduceSupply(amount));

    public bool CanBeRemoved() => Supply == 0;
}


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
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .Process(unitType => unitType.Supply += amount)
            .MergeResultErrors(unitType => unitType.IssuerAccount.IncreaseSupply(amount));

    public Result<UnitType> ReduceSupply(decimal amount, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .IfTrueAddError(Supply < amount, Errors.AmountIsGreaterThanSupply(cultureInfo))
            .Process(unitType => unitType.Supply -= amount)
            .MergeResultErrors(unitType => unitType.IssuerAccount.ReduceSupply(amount));

    public bool CanBeRemoved() => Supply == 0;
}


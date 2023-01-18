using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;

namespace Vouchers.Domains.Domain;

public sealed class DomainOffer : AggregateRoot<Guid>
{
    public string Name { get; init; }
    public string Description { get; set; }

    public int MaxMembersCount { get; init; }
    public CurrencyAmount Amount { get; init; }
    public InvoicePeriod InvoicePeriod { get; init; }

    public bool IsPublic { get; init; }
    public Guid? RecipientId { get; init; }

    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public int? MaxContractsPerIdentity { get; init; }

    public static DomainOffer Create(Guid id, string name, string description, int maxMembersCount,
        CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo,
        int? maxContractsPerIdentity) => Create(id, name, description, maxMembersCount,
        amount, period, validFrom, validTo,
        maxContractsPerIdentity, null);

    public static DomainOffer Create(Guid id, string name, string description, int maxMembersCount,
        CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo,
        int? maxContractsPerIdentity, Guid? recipientId) => new()
    {
        Id = id,
        Name = name,
        Description = description,
        MaxMembersCount = maxMembersCount,
        Amount = amount,
        InvoicePeriod = period,
        ValidFrom = validFrom,
        ValidTo = validTo,
        MaxContractsPerIdentity = maxContractsPerIdentity,
        RecipientId = recipientId,
    };
}

public class CurrencyAmount
{
    public Currency Currency { get; }
    public decimal Amount { get; }

    private CurrencyAmount(Currency currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public static CurrencyAmount Create(Currency currency, decimal amount)
    {
        return new CurrencyAmount(currency, amount);
    }
}

public enum Currency
{
    USD,
    EUR,
    CZK,
    RUB
}

public enum InvoicePeriod
{
    MONTH,
    QUARTER,
    YEAR
}


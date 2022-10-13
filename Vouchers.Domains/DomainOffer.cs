using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Domains
{
    public sealed class DomainOffer : Entity
    {
        public string Name { get; }
        public string Description { get; set; }

        public int MaxSubscribersCount { get; }
        public CurrencyAmount Amount { get; }
        public InvoicePeriod InvoicePeriod { get; }

        public bool IsPublic { get; }
        public Guid? RecipientId { get; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public int? MaxContractsPerIdentity { get; }

        public static DomainOffer Create(string name, string description, int maxMembersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, int? maxContractsPerIdentity) =>
            new DomainOffer(Guid.NewGuid(), name, description, maxMembersCount, amount, period, validFrom, validTo, maxContractsPerIdentity);

        public static DomainOffer Create(string name, string description, int maxMembersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, Guid recipientId, int? maxContractsPerIdentity) =>
            new DomainOffer(Guid.NewGuid(), name, description, maxMembersCount, amount, period, validFrom, validTo, recipientId, maxContractsPerIdentity);

        public DomainOffer(Guid id, string name, string description, int maxSubscribersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, int? maxContractsPerIdentity) : base(id)
        {
            Name = name;
            Description = description;
            MaxSubscribersCount = maxSubscribersCount;
            Amount = amount;
            InvoicePeriod = period;
            ValidFrom = validFrom;
            ValidTo = validTo;
            MaxContractsPerIdentity = maxContractsPerIdentity;
        }

        public DomainOffer(Guid id, string name, string description, int maxSubscribersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, Guid recipientId, int? maxContractsPerIdentity)
            : this(id, name, description, maxSubscribersCount, amount, period, validFrom, validTo, maxContractsPerIdentity) =>
            RecipientId = recipientId;


        private DomainOffer()
        {}
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
}

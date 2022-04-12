using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;

namespace Vouchers.Domains
{
    public class DomainOffer
    {
        public Guid Id { get; }

        public string Name { get; }
        public string Description { get; set; }

        public int MaxSubscribersCount { get; }
        public CurrencyAmount Amount { get; }
        public InvoicePeriod InvoicePeriod { get; }

        public bool IsPublic { get; }
        public Identity Recipient { get; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public static DomainOffer Create(string name, string description, int maxMembersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo) =>
            new DomainOffer(Guid.NewGuid(), name, description, maxMembersCount, amount, period, validFrom, validTo);

        public static DomainOffer Create(string name, string description, int maxMembersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, Identity recipient) =>
            new DomainOffer(Guid.NewGuid(), name, description, maxMembersCount, amount, period, validFrom, validTo, recipient);

        public DomainOffer(Guid id, string name, string description, int maxSubscribersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo)
        {
            Id = id;
            Name = name;
            Description = description;
            MaxSubscribersCount = maxSubscribersCount;
            Amount = amount;
            InvoicePeriod = period;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public DomainOffer(Guid id, string name, string description, int maxSubscribersCount, CurrencyAmount amount, InvoicePeriod period, DateTime validFrom, DateTime validTo, Identity recipient)
            : this(id, name, description, maxSubscribersCount, amount, period, validFrom, validTo) =>
            Recipient = recipient;


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
        YEAR
    }
}

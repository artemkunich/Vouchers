using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;
using System.Globalization;

namespace Vouchers.Core
{
    public sealed class IssuerTransaction : Entity<Guid>
    {
        public DateTime Timestamp { get; private set; }

        public UnitQuantity Quantity { get; }

        public Guid IssuerAccountItemId { get; }
        public AccountItem IssuerAccountItem { get; }

        public static IssuerTransaction Create(AccountItem issuerAccountItem, decimal amount, CultureInfo cultureInfo = null) =>
            new IssuerTransaction(Guid.NewGuid(), DateTime.Now, issuerAccountItem, UnitQuantity.Create(amount, issuerAccountItem.Unit), cultureInfo);

        private IssuerTransaction(Guid id, DateTime timestamp, AccountItem issuerAccountItem, UnitQuantity quantity, CultureInfo cultureInfo = null) : base(id)
        {
            Timestamp = timestamp;

            IssuerAccountItemId = issuerAccountItem.Id;
            IssuerAccountItem = issuerAccountItem;
            
            Quantity = quantity;

            if (Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException("UnitIsExpired", cultureInfo);

            if (Quantity.Unit.UnitType.IssuerAccount.NotEquals(IssuerAccountItem.HolderAccount))
                throw new CoreException("AccountHolderAndUnitTypeIssuerAreDifferent", cultureInfo);

            if (Quantity.Unit.NotEquals(IssuerAccountItem.Unit))
                throw new CoreException("AccountItemUnitAndTransactionUnitAreDifferent", cultureInfo);


            if (Quantity.Amount == 0)
                throw new CoreException("AmountIsNotPositive", cultureInfo);

        }

        private IssuerTransaction() { }

        public void Perform()
        {
            if (Quantity.Amount > 0)
            {
                IssuerAccountItem.ProcessDebit(Quantity.Amount);
                IssuerAccountItem.Unit.IncreaseSupply(Quantity.Amount);
            }
            else
            {
                IssuerAccountItem.ProcessCredit(-Quantity.Amount);
                IssuerAccountItem.Unit.ReduceSupply(-Quantity.Amount);
            }
        }
    }
}

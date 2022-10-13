using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public sealed class IssuerTransaction : Entity
    {
        public DateTime Timestamp { get; private set; }

        public UnitQuantity Quantity { get; }

        public Guid IssuerAccountItemId { get; }
        public AccountItem IssuerAccountItem { get; }

        public static IssuerTransaction Create(AccountItem issuerAccountItem, decimal amount) =>
            new IssuerTransaction(Guid.NewGuid(), DateTime.Now, issuerAccountItem, UnitQuantity.Create(amount, issuerAccountItem.Unit));

        internal IssuerTransaction(Guid id, DateTime timestamp, AccountItem issuerAccountItem, UnitQuantity quantity) : base(id)
        {
            Timestamp = timestamp;

            IssuerAccountItemId = issuerAccountItem.Id;
            IssuerAccountItem = issuerAccountItem;
            
            Quantity = quantity;
            //new VoucherQuantity(quantity.Amount, issuerAccount.Unit);

            if (Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException($"Voucher {Quantity.Unit.Id} is expired");

            if (Quantity.Unit.UnitType.IssuerAccount.NotEquals(IssuerAccountItem.HolderAccount))
                throw new CoreException($"Account owner {IssuerAccountItem.HolderAccountId} is not issuer of voucher {Quantity.Unit.UnitType.Id}");

            if (Quantity.Unit.NotEquals(IssuerAccountItem.Unit))
                throw new CoreException($"Account unit {IssuerAccountItem.UnitId} is not voucher {Quantity.Unit.UnitType.Id}");


            if (Quantity.Amount == 0)
                throw new CoreException("Transaction cannot have amount 0");

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

using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class IssuerTransaction : Entity
    {
        public DateTime Timestamp { get; private set; }

        public UnitQuantity Quantity { get; }

        public Guid IssuerAccountId { get; }
        public AccountItem IssuerAccount { get; }

        public static IssuerTransaction Create(AccountItem issuerAccount, decimal amount) =>
            new IssuerTransaction(Guid.NewGuid(), DateTime.Now, issuerAccount, UnitQuantity.Create(amount, issuerAccount.Unit));

        internal IssuerTransaction(Guid id, DateTime timestamp, AccountItem issuerAccount, UnitQuantity quantity) : base(id)
        {
            Timestamp = timestamp;

            IssuerAccountId = issuerAccount.Id;
            IssuerAccount = issuerAccount;
            
            Quantity = quantity;
            //new VoucherQuantity(quantity.Amount, issuerAccount.Unit);

            if (Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException($"Voucher {Quantity.Unit.Id} is expired");

            if (Quantity.Unit.UnitType.Issuer.NotEquals(IssuerAccount.Holder))
                throw new CoreException($"Account owner {IssuerAccount.HolderId} is not issuer of voucher {Quantity.Unit.UnitType.Id}");

            if (Quantity.Unit.NotEquals(IssuerAccount.Unit))
                throw new CoreException($"Account unit {IssuerAccount.UnitId} is not voucher {Quantity.Unit.UnitType.Id}");


            if (Quantity.Amount == 0)
                throw new CoreException("Transaction cannot have amount 0");

        }

        private IssuerTransaction() { }

        public void Perform()
        {
            if (Quantity.Amount > 0)
            {
                IssuerAccount.ProcessDebit(Quantity.Amount);
                IssuerAccount.Unit.IncreaseSupply(Quantity.Amount);
            }
            else
            {
                IssuerAccount.ProcessCredit(-Quantity.Amount);
                IssuerAccount.Unit.ReduceSupply(-Quantity.Amount);
            }
        }
    }
}

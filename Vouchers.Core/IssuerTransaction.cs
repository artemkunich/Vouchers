using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class IssuerTransaction
    {
        public Guid Id { get; }

        public DateTime Timestamp { get; private set; }

        public VoucherQuantity Quantity { get; }

        public VoucherAccount IssuerAccount { get; }

        public static IssuerTransaction Create(VoucherAccount issuerAccount, decimal amount) =>
            new IssuerTransaction(Guid.NewGuid(), DateTime.Now, issuerAccount, VoucherQuantity.Create(amount, issuerAccount.Unit));

        internal IssuerTransaction(Guid id, DateTime timestamp, VoucherAccount issuerAccount, VoucherQuantity quantity) : this(timestamp, issuerAccount, quantity) =>
            Id = id;

        internal IssuerTransaction(DateTime timestamp, VoucherAccount issuerAccount, VoucherQuantity quantity)
        {
            Timestamp = timestamp;
            IssuerAccount = issuerAccount;
            Quantity = quantity;
            //new VoucherQuantity(quantity.Amount, issuerAccount.Unit);

            if (Quantity.Unit.ValidTo < DateTime.Today)
                throw new CoreException($"Voucher {Quantity.Unit.Id} is expired");

            if (Quantity.Unit.Value.Issuer.NotEquals(IssuerAccount.Holder))
                throw new CoreException($"Account owner {IssuerAccount.Holder.Id} is not issuer of voucher {Quantity.Unit.Value.Id}");

            if (Quantity.Unit.NotEquals(IssuerAccount.Unit))
                throw new CoreException($"Account unit {IssuerAccount.Unit.Id} is not voucher {Quantity.Unit.Value.Id}");


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

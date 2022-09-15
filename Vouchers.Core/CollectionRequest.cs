using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class CollectionRequest : Entity
    {
        public DateTime RequestValidTo { get; }

        public Guid CreditorId { get; }
        public Account Creditor { get; }

        public Guid DebtorId { get; }
        public Account Debtor { get; }     

        public UnitTypeQuantity Quantity { get; }

        public DateTime MaxValidFrom { get; }
        public DateTime MinValidTo { get; }
        
        public bool MustBeExchangeable { get; }

        public HolderTransaction Collection { get; private set; }

        public static CollectionRequest Create(DateTime validTo, Account debtor, UnitTypeQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable) =>
            new CollectionRequest(Guid.NewGuid(), validTo, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable);

        public static CollectionRequest Create(DateTime validTo, Account creditor, Account debtor, UnitTypeQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable) =>
            new CollectionRequest(Guid.NewGuid(), validTo, creditor, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable);


        public CollectionRequest(Guid id, DateTime requestValidTo, Account debtor, UnitTypeQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable) : base(id)
        {
            if (quantity.Amount <= 0)
                throw new CoreException("Amount must be greater than 0");

            RequestValidTo = requestValidTo;

            DebtorId = debtor.Id;
            Debtor = debtor;

            Quantity = quantity;
            MaxValidFrom = maxValidFrom;
            MinValidTo = minValidTo;
            MustBeExchangeable = mustBeExchangeable;
        }

        public CollectionRequest (Guid id, DateTime validTo, Account creditor, Account debtor, UnitTypeQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable): this(id, validTo, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable)
        {
            if (creditor.Equals(debtor))
                throw new CoreException("Creditor and debtor can not be the same");

            CreditorId = creditor.Id;
            Creditor = creditor;            
        }

        public void PerformCollection(HolderTransaction collection)
        {
            if(Collection != null)
                throw new CoreException($"Collection is already performed");

            if (Creditor != null && Creditor.NotEquals(collection.Creditor))
                throw new CoreException($"Request's creditor is not satisfied by collection");

            if (Debtor.NotEquals(collection.Debtor))
                throw new CoreException($"Request's debtor is not satisfied by collection");

            if (Quantity.UnitType.NotEquals(collection.Quantity.UnitType))
                throw new CoreException($"Request's unit is not satisfied by collection");

            if (Quantity.Amount == collection.Quantity.Amount)
                throw new CoreException($"Request's amount is not satisfied by collection");

            foreach (var item in collection.TransactionItems)
            {
                if (item.Quantity.Unit.ValidFrom > MaxValidFrom)
                    throw new CoreException($"Request's maxValidFrom is not satisfied by collection");

                if (item.Quantity.Unit.ValidFrom < MinValidTo)
                    throw new CoreException($"Request's minValidTo is not satisfied by collection");

                if (MustBeExchangeable && !item.Quantity.Unit.CanBeExchanged)
                    throw new CoreException($"Request's mustBeExchangeable is not satisfied by collection");
            }

            collection.Perform();
            Collection = collection;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class CollectionRequest
    {
        public Guid Id { get; }

        public DateTime RequestValidTo { get; }

        public DomainAccount Creditor { get; }
        public DomainAccount Debtor { get; }     

        public VoucherValueQuantity Quantity { get; }

        public DateTime MaxValidFrom { get; }
        public DateTime MinValidTo { get; }
        
        public bool MustBeExchangeable { get; }

        public HolderTransaction Collection { get; private set; }

        public static CollectionRequest Create(DateTime validTo, DomainAccount debtor, VoucherValueQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable) =>
            new CollectionRequest(validTo, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable);

        public static CollectionRequest Create(DateTime validTo, DomainAccount creditor, DomainAccount debtor, VoucherValueQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable) =>
            new CollectionRequest(validTo, creditor, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable);


        public CollectionRequest(DateTime requestValidTo, DomainAccount debtor, VoucherValueQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable)
        {
            if (quantity.Amount <= 0)
                throw new CoreException("Amount must be greater than 0");

            RequestValidTo = requestValidTo;
            Debtor = debtor;
            Quantity = quantity;
            MaxValidFrom = maxValidFrom;
            MinValidTo = minValidTo;
            MustBeExchangeable = mustBeExchangeable;
        }

        public CollectionRequest (DateTime validTo, DomainAccount creditor, DomainAccount debtor, VoucherValueQuantity quantity, DateTime maxValidFrom, DateTime minValidTo, bool mustBeExchangeable): this(validTo, debtor, quantity, maxValidFrom, minValidTo, mustBeExchangeable)
        {
            if (creditor.Equals(debtor))
                throw new CoreException("Creditor and debtor can not be the same");

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

            if (Quantity.Unit.NotEquals(collection.Quantity.Unit))
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

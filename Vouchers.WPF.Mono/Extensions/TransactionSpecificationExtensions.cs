using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Views.Specifications;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class TransactionSpecificationExtensions
    {
        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionMinAmount specification)
        {
            return new MinAmountSpecification(specification.MinAmount);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionMaxAmount specification)
        {
            return new MaxAmountSpecification(specification.MaxAmount);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionTimestamp specification)
        {
            return new MinTimestampSpecification(specification.MinTimestamp).And(new MaxTimestampSpecification(specification.MaxTimestamp));
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionCounterparty specification)
        {
            return new TransactionCounterpartySpecification(specification.CounterpartyId);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionValueIssuer specification)
        {
            return new ValueIssuerSpecification(specification.IssuerId);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionValueCategory specification)
        {
            return new ValueCategorySpecification(specification.Category);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.TransactionValueName specification)
        {
            return new ValueNameSpecification(specification.Name);
        }
    }
}

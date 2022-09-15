using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Views.Specifications;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class UserAccountSpecificationExtensions
    {
        public static Specification GetVouchersSpecification(this Model.Specifications.UserAccountId specification)
        {
            return new UserAccountIdSpecification(specification.Id);
        }

        public static Specification GetVouchersSpecification(this Model.Specifications.UserAccountEmail specification)
        {
            return new EmailSpecification(specification.Email);
        }
    }
}

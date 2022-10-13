using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries
{
    public sealed class IdentityDomainAccountsQuery : ListQuery
    {
        public string DomainName { get; set; }
    }
}

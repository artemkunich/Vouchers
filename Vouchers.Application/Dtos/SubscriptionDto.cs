using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class SubscriptionDto
    {
        public Guid DomainAccountId { get; set; }

        public string DomainName { get; set; }
    }
}

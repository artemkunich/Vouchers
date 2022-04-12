using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries
{
    public class SubscribersQuery
    {
        [Required]
        public Guid DomainId { get; set; }

        public string IdentityName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

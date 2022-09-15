using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries
{
    public class DomainAccountsQuery
    {
        [Required]
        public Guid DomainId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool IncludeConfirmed { get; set; } = true;

        public bool IncludeNotConfirmed { get; set; } = false;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries
{
    public class DomainQuery
    {
        [Required]
        public Guid DomainId { get; set; }
    }
}

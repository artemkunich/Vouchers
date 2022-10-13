using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Dtos
{
    public sealed class VoucherDto
    {
        public Guid Id { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public bool CanBeExchanged { get; set; }

        public decimal Supply { get; set; }

        public decimal Balance { get; set; }
    }
}

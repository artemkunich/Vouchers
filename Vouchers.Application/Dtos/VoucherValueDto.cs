using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Dtos
{
    public sealed class VoucherValueDto
    {
        public Guid Id { get; set; }

        public Guid IssuerAccountId { get; set; }

        public string IssuerName { get; set; }

        public string IssuerEmail { get; set; }

        public decimal Supply { get; set; }

        public decimal Balance { get; set; }

        public string Ticker { get; set; }

        public string Description { get; set; }

        public Guid? ImageId { get; set; }

    }
}

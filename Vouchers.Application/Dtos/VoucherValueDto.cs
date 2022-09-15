using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Dtos
{
    public class VoucherValueDto
    {
        public Guid Id { get; set; }

        public Guid IssuerId { get; set; }

        public decimal Supply { get; set; }

        public decimal Balance { get; set; }

        public string Ticker { get; set; }

        public string Description { get; set; }

        public string ImageBase64 { get; set; }

        public ICollection<VoucherDto> Vouchers { get; set; }
    }
}

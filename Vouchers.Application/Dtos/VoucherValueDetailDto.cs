using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public sealed class VoucherValueDetailDto
    {
        public string Ticker { get; set; }

        public string Description { get; set; }

        public Guid? ImageId { get; set; }

        public CropParametersDto CropParameters { get; set; }
    }
}

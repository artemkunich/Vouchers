using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Dtos
{
    public class VoucherValueDetailDto
    {
        public string Ticker { get; set; }

        public string Description { get; set; }

        public string ImageBase64 { get; set; }

        public CropParametersDto CropParameters { get; set; }
    }
}

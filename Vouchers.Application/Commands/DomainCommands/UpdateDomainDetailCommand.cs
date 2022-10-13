using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainCommands
{
    public sealed class UpdateDomainDetailCommand
    {
        public Guid DomainId { get; set; }

        public DomainDetailDto DomainDetail { get; set; }

        public IFormFile Image { get; set; }
    }
}

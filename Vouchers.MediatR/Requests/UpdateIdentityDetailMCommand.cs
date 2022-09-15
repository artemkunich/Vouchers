using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands;
using Vouchers.Application.UseCases;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class UpdateIdentityDetailMCommand : AuthorizedRequest<Unit>
    {
        public UpdateIdentityDetailCommand Command { get; set; }
    }
}
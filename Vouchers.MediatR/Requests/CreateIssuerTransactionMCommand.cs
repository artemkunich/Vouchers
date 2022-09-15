using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class CreateIssuerTransactionMCommand : AuthorizedRequest<Unit>
    {
        public CreateIssuerTransactionCommand Command { get; set; }
    }
}
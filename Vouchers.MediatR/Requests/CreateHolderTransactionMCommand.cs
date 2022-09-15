using System;
using System.Collections.Generic;
using MediatR;
using System.Text;
using Vouchers.Application.Commands;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class CreateHolderTransactionMCommand : AuthorizedRequest<Unit>
    {
        public CreateHolderTransactionCommand Command { get; set; }
    }
}

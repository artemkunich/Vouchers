using MediatR;
using System;
using System.Collections.Generic;
using Vouchers.Application.Commands;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class CreateCollectionMCommand : AuthorizedRequest<Unit>
    {
        public CreateCollectionCommand Command { get; set; }
    }
}

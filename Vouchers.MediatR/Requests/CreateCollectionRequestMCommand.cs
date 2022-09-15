using MediatR;
using System;
using Vouchers.Application.Commands;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class CreateCollectionRequestMCommand : AuthorizedRequest<Unit>
    {
        public CreateCollectionRequestCommand Command { get; set; }
    }
}

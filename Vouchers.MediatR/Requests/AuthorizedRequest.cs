using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class AuthorizedRequest<T> : IRequest<T>
    {
        public Guid OwnerId { get; set; }
    }
}

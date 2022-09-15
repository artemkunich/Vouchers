using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.MediatR.Requests
{
    public class Request<T> : IRequest<T>
    {
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands;
using Vouchers.Application.UseCases;

namespace Vouchers.MediatR.Requests
{
    public class CreateIdentityMCommand : Request<Unit>
    {
        public CreateIdentityCommand Command { get; set; }
    }
}
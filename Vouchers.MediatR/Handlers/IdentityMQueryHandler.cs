using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.UseCases;
using Vouchers.MediatR.Requests;

namespace Vouchers.MediatR.Handlers
{
    public class IdentityMQueryHandler : IRequestHandler<IdentityMQuery, Guid?>
    {
        private readonly IHandler<string, Guid?> appHandler;

        public IdentityMQueryHandler(IHandler<string, Guid?> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<Guid?> Handle(IdentityMQuery request, CancellationToken cancellationToken)
        {
            return await appHandler.HandleAsync(request.LoginName);
        }
    }
}

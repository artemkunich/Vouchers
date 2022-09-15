using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands;
using Vouchers.Application.UseCases;
using Vouchers.MediatR.Requests;

namespace Vouchers.MediatR.Handlers
{
    public class CreateIssuerTransactionMCommandHandler //: IRequestHandler<CreateIssuerTransactionMCommand>
    {
        private readonly IAuthIdentityHandler<CreateIssuerTransactionCommand> appHandler;

        public CreateIssuerTransactionMCommandHandler(IAuthIdentityHandler<CreateIssuerTransactionCommand> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<Unit> Handle(CreateIssuerTransactionMCommand request, CancellationToken cancellationToken)
        {
            await appHandler.HandleAsync(request.Command, request.OwnerId);
            return Unit.Value;
        }
    }
}
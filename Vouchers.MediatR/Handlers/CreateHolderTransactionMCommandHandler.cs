using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Vouchers.Application.Commands;
using Vouchers.Application.UseCases;
using Vouchers.MediatR.Requests;

namespace Vouchers.MediatR.Handlers
{
    public class CreateHolderTransactionMCommandHandler //: IRequestHandler<CreateHolderTransactionMCommand>
    {
        private readonly IAuthIdentityHandler<CreateHolderTransactionCommand> appHandler;

        public CreateHolderTransactionMCommandHandler(IAuthIdentityHandler<CreateHolderTransactionCommand> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<Unit> Handle(CreateHolderTransactionMCommand request, CancellationToken cancellationToken)
        {
            await appHandler.HandleAsync(request.Command, request.OwnerId);
            return Unit.Value;
        }
    }
}
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
    public class CreateCollectionRequestMCommandHandler //: IRequestHandler<CreateCollectionRequestMCommand>
    {
        private readonly IAuthIdentityHandler<CreateCollectionRequestCommand> appHandler;

        public CreateCollectionRequestMCommandHandler(IAuthIdentityHandler<CreateCollectionRequestCommand> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<Unit> Handle(CreateCollectionRequestMCommand request, CancellationToken cancellationToken)
        {
            await appHandler.HandleAsync(request.Command, request.OwnerId);
            return Unit.Value;
        }
    }
}
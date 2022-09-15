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
    public class CreateIdentityMCommandHandler : IRequestHandler<CreateIdentityMCommand>
    {
        private readonly IHandler<CreateIdentityCommand> appHandler;

        public CreateIdentityMCommandHandler(IHandler<CreateIdentityCommand> appHandler) 
        {
            this.appHandler = appHandler;
        }

        public async Task<Unit> Handle(CreateIdentityMCommand request, CancellationToken cancellationToken)
        {
            await appHandler.HandleAsync(request.Command);
            return Unit.Value;
        }
    }
}
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
    public class IdentityDetailMQueryHandler : IRequestHandler<IdentityDetailMQuery, IdentityDetailDto>
    {
        private readonly IAuthIdentityHandler<Guid, IdentityDetailDto> appHandler;

        public IdentityDetailMQueryHandler(IAuthIdentityHandler<Guid, IdentityDetailDto> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<IdentityDetailDto> Handle(IdentityDetailMQuery request, CancellationToken cancellationToken)
        {
            return await appHandler.HandleAsync(request.OwnerId, request.OwnerId);
        }
    }
}

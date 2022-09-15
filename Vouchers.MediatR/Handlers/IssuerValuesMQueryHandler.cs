using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MediatR.Requests;

namespace Vouchers.MediatR.Handlers
{
    public class IssuerValuesMQueryHandler //: IRequestHandler<IssuerValuesMQuery, IEnumerable<VoucherValueDto>>
    {
        private readonly IAuthIdentityHandler<Guid, IEnumerable<VoucherValueDto>> appHandler;

        public IssuerValuesMQueryHandler(IAuthIdentityHandler<Guid, IEnumerable<VoucherValueDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IEnumerable<VoucherValueDto>> Handle(IssuerValuesMQuery request, CancellationToken cancellationToken) =>
            await appHandler.HandleAsync(request.DomainAccountId, request.OwnerId);
    }
}

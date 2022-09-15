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
    public class HolderValuesMQueryHandler //: IRequestHandler<HolderValuesMQuery, IEnumerable<VoucherValueDto>>
    {
        private readonly IAuthIdentityHandler<Guid, IEnumerable<VoucherValueDto>> appHandler;

        public HolderValuesMQueryHandler(IAuthIdentityHandler<Guid, IEnumerable<VoucherValueDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IEnumerable<VoucherValueDto>> Handle(HolderValuesMQuery request, CancellationToken cancellationToken) =>
            await appHandler.HandleAsync(request.DomainAccountId, request.OwnerId);
    }
}

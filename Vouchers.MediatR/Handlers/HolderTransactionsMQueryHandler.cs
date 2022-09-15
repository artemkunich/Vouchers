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
    public class HolderTransactionsMQueryHandler //: IRequestHandler<HolderTransactionsMQuery, IEnumerable<HolderTransactionDto>>
    {
        private readonly IAuthIdentityHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>> appHandler;

        public HolderTransactionsMQueryHandler(IAuthIdentityHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IEnumerable<HolderTransactionDto>> Handle(HolderTransactionsMQuery request, CancellationToken cancellationToken) =>
            await appHandler.HandleAsync(request.AppQuery, request.OwnerId);
    }
}

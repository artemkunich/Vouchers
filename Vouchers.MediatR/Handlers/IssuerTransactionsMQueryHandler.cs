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
    public class IssuerTransactionsMQueryHandler //: IRequestHandler<IssuerTransactionsMQuery, IEnumerable<IssuerTransactionDto>>
    {
        private readonly IAuthIdentityHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>> appHandler;

        public IssuerTransactionsMQueryHandler(IAuthIdentityHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IEnumerable<IssuerTransactionDto>> Handle(IssuerTransactionsMQuery request, CancellationToken cancellationToken) =>
            await appHandler.HandleAsync(request.AppQuery, request.OwnerId);
    }
}

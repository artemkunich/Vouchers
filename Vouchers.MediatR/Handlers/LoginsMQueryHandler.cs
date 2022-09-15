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
    public class LoginsMQueryHandler : IRequestHandler<LoginsMQuery, IPaginatedEnumerable<LoginDto>>
    {
        private readonly IHandler<LoginsQuery, IPaginatedEnumerable<LoginDto>> appHandler;

        public LoginsMQueryHandler(IHandler<LoginsQuery, IPaginatedEnumerable<LoginDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IPaginatedEnumerable<LoginDto>> Handle(LoginsMQuery request, CancellationToken cancellationToken)
        {
            return await appHandler.HandleAsync(request.AppQuery);
        }
    }
}

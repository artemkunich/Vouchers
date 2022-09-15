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
    public class DomainOffersMQueryHandler : IRequestHandler<DomainOffersMQuery, IPaginatedEnumerable<DomainOfferDto>>
    {
        private readonly IHandler<DomainOffersQuery, IPaginatedEnumerable<DomainOfferDto>> appHandler;

        public DomainOffersMQueryHandler(IHandler<DomainOffersQuery, IPaginatedEnumerable<DomainOfferDto>> appHandler) =>
            this.appHandler = appHandler;

        public async Task<IPaginatedEnumerable<DomainOfferDto>> Handle(DomainOffersMQuery request, CancellationToken cancellationToken) =>
            await appHandler.HandleAsync(request.AppQuery);
    }
}

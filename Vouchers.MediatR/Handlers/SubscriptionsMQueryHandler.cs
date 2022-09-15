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
    public class SubscriptionsMQueryHandler : IRequestHandler<SubscriptionsMQuery, IEnumerable<SubscriptionDto>>
    {
        private readonly IAuthIdentityHandler<SubscriptionsQuery, IEnumerable<SubscriptionDto>> appHandler;

        public SubscriptionsMQueryHandler(IAuthIdentityHandler<SubscriptionsQuery, IEnumerable<SubscriptionDto>> appHandler)
        {
            this.appHandler = appHandler;
        }

        public async Task<IEnumerable<SubscriptionDto>> Handle(SubscriptionsMQuery request, CancellationToken cancellationToken)
        {
            return await appHandler.HandleAsync(request.AppQuery, request.OwnerId);
        }
    }
}

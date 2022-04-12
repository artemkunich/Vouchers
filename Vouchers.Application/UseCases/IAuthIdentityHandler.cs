using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.UseCases
{
    public interface IAuthIdentityHandler<in TRequest>
    {
        Task HandleAsync(TRequest request, Guid authIdentityId, CancellationToken cancellation);
    }

    public interface IAuthIdentityHandler<in TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, Guid authIdentityId, CancellationToken cancellation);
    }
}

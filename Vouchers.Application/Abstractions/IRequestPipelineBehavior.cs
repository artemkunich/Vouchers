using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public delegate Task<Result<TResponse>> HandlerDelegate<TResponse>();

public interface IRequestPipelineBehavior<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> nextAsync);
}
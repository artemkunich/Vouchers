using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.UseCases;

public interface IHandler<in TRequest>
{
    Task<Result> HandleAsync(TRequest request, CancellationToken cancellation);
}

public interface IHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}
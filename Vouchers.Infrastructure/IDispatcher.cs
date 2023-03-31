using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Infrastructure;

public interface IDispatcher
{
    Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default) where TRequest: IRequest<TResult>;
}
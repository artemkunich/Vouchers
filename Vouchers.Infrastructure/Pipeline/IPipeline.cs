using Vouchers.Application.Abstractions;

namespace Vouchers.Infrastructure.Pipeline;

public interface IPipeline<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Common.Application.Infrastructure;

public interface IDomainEventDispatcher
{
    Task<Result<Unit>> DispatchAsync<TEvent>(TEvent request, CancellationToken cancellation = default) where TEvent: IDomainEvent;
}
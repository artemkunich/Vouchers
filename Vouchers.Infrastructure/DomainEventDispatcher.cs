using Microsoft.Extensions.DependencyInjection;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.Infrastructure;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task<Result<Unit>> DispatchAsync<TEvent>(TEvent request, CancellationToken cancellation = default) where TEvent: IDomainEvent
    {
        var eventHandlers = _serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
        foreach (var eventHandler in eventHandlers)
        {
            var result = await eventHandler.HandleAsync(request, cancellation);
            if (result.IsFailure)
                return result;
        }
        
        return Unit.Value;
    }
}
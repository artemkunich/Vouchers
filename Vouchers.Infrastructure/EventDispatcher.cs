using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.Infrastructure;

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

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
using System.Text.Json;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.MinimalAPI.EventRouters;

public sealed class GenericEventRouter<T>: IEventRouter
{
    private readonly IDispatcher _dispatcher;

    public GenericEventRouter(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task RouteAsync(string eventJson, CancellationToken token)
    {
        var @event = JsonSerializer.Deserialize<T>(eventJson);
        await _dispatcher.DispatchAsync(@event, token);
    }
}
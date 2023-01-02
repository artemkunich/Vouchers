using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Infrastructure;

namespace Vouchers.API.EventRouters;

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
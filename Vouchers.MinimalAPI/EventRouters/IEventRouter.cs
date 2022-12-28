namespace Vouchers.MinimalAPI.EventRouters;

public interface IEventRouter
{
    Task RouteAsync(string @eventJson, CancellationToken token);
}
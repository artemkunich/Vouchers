using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.API.EventRouters;

public interface IEventRouter
{
    Task RouteAsync(string @eventJson, CancellationToken token);
}
using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation);
}
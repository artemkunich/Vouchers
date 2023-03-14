using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public interface IIntegrationEventPipelineBehavior<in TEvent>
{
    Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync);
}
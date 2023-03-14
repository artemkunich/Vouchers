using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation);
}
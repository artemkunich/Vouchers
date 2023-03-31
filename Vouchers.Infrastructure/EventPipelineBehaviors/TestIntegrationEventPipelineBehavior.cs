using Vouchers.Common.Application.Abstractions;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Infrastructure.EventPipelineBehaviors;

public class TestIntegrationEventPipelineBehavior<TMessage> : IIntegrationEventPipelineBehavior<TMessage>
{
    public Task<Result<Unit>> HandleAsync(TMessage @event, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync)
    {
        return nextAsync();
    }
}
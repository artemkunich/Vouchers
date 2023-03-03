using Vouchers.Application.Abstractions;

namespace Vouchers.Infrastructure.EventPipelineBehaviors;

public class TestEventPipelineBehavior<TMessage> : IEventPipelineBehavior<TMessage>
{
    public Task<Result<Unit>> HandleAsync(TMessage request, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync)
    {
        return nextAsync();
    }
}
using Vouchers.Primitives;

namespace Vouchers.Infrastructure;

public interface IMessageHandler<in TMessage,TResult>
{
    Task HandleAsync(TMessage message, CancellationToken token);
}

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken token);
}
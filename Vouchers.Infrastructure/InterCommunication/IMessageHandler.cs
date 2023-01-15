namespace Vouchers.Infrastructure.InterCommunication;

public interface IMessageHandler<in TMessage,TResult>
{
    Task HandleAsync(TMessage message, CancellationToken token);
}

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken token);
}
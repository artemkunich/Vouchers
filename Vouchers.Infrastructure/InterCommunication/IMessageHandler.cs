namespace Vouchers.Infrastructure.InterCommunication;

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken cancellation);
}
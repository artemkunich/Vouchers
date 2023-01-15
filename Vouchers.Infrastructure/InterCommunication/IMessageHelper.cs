namespace Vouchers.Infrastructure.InterCommunication;

public interface IMessageHelper
{
    public Guid? GetMessageId(object message);
    public Task<bool> CheckIfMessageWasConsumedAsync(Guid messageId, string consumer);
}
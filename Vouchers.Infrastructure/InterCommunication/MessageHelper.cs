using Microsoft.EntityFrameworkCore;
using Vouchers.Persistence;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Infrastructure.InterCommunication;

public class MessageHelper : IMessageHelper
{
    private VouchersDbContext _dbContext;

    public MessageHelper(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Guid? GetMessageId(object message)
    {
        var idPropertyInfo = message.GetType().GetProperty("Id", typeof(Guid));
        if (idPropertyInfo is null)
            return null;

        return idPropertyInfo.GetValue(message) as Guid?;
    }

    public async Task<bool> CheckIfMessageWasConsumedAsync(Guid messageId, string consumer)
    {
        var consumedMessage = await _dbContext.Set<ConsumedMessage>()
            .FirstOrDefaultAsync(m => m.MessageId == messageId && m.Consumer == consumer);

        if (consumedMessage is null)
            return false;

        return true;

    }
}
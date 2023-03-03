using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vouchers.Persistence.InterCommunication;

public class MessageHelper : IMessageHelper
{
    private readonly VouchersDbContext _dbContext;

    public MessageHelper(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Guid? GetMessageId(object message)
    {
        var idPropertyInfo = message.GetType().GetProperty("Id", typeof(Guid));
        return idPropertyInfo?.GetValue(message) as Guid?;
    }

    public async Task<bool> CheckIfMessageWasConsumedAsync(Guid messageId, string consumer)
    {
        var consumedMessage = await _dbContext.Set<ConsumedMessage>()
            .FirstOrDefaultAsync(m => m.MessageId == messageId && m.Consumer == consumer);

        return consumedMessage is not null;
    }
}
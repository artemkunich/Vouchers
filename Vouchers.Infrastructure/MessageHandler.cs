using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Vouchers.Application.UseCases;
using Vouchers.Persistence;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Primitives;

namespace Vouchers.Infrastructure;

public class MessageHandler<TMessage,TResult> : IMessageHandler<TMessage,TResult>
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IHandler<TMessage, TResult> _handler;
    
    public MessageHandler(IMessageHelper messageHelper, VouchersDbContext dbContext, IHandler<TMessage, TResult> handler)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _handler = handler;
    }
    
    public async Task HandleAsync(TMessage message, CancellationToken token)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return;

        var consumer = _handler.GetType().Name;
        
        var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
        if (isMessageConsumed)
        {
            return;
        }

        _dbContext.Set<ConsumedMessage>().Add(ConsumedMessage.Create(messageId.Value, consumer));
        
        TResult result = await _handler.HandleAsync(message, token);

        if (_dbContext.ChangeTracker.HasChanges())
            await _dbContext.SaveChangesAsync(token);
    }
}

public class MessageHandler<TMessage> : IMessageHandler<TMessage>
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IHandler<TMessage> _handler;
    
    public MessageHandler(IMessageHelper messageHelper, VouchersDbContext dbContext, IHandler<TMessage> handler)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _handler = handler;
    }

    public async Task HandleAsync(TMessage message, CancellationToken token)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return;

        var consumer = _handler.GetType().Name;

        var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
        if (isMessageConsumed)
        {
            return;
        }

        _dbContext.Set<ConsumedMessage>().Add(ConsumedMessage.Create(messageId.Value, consumer));

        await _handler.HandleAsync(message, token);

        if (_dbContext.ChangeTracker.HasChanges())
            await _dbContext.SaveChangesAsync(token);
    }
}

    
public interface IMessageHelper
{
    public Guid? GetMessageId(object message);
    public Task<bool> CheckIfMessageWasConsumedAsync(Guid messageId, string consumer);
}


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
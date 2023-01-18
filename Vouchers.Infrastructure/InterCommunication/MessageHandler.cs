using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Persistence;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Infrastructure.InterCommunication;

public class MessageHandler<TMessage,TResult> : IMessageHandler<TMessage,TResult>
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IHandler<TMessage, TResult> _handler;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public MessageHandler(IMessageHelper messageHelper, VouchersDbContext dbContext, IHandler<TMessage, TResult> handler, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _handler = handler;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task HandleAsync(TMessage message, CancellationToken token)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return;

        var consumer = _handler.GetType().FullName;
        
        var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
        if (isMessageConsumed)
        {
            return;
        }

        var messageConsumerId = _identifierProvider.CreateNewId();
        var messageConsumer = ConsumedMessage.Create(messageConsumerId, messageId.Value, consumer, _dateTimeProvider.CurrentDateTime());
        _dbContext.Set<ConsumedMessage>().Add(messageConsumer);
        
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
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    public MessageHandler(IMessageHelper messageHelper, VouchersDbContext dbContext, IHandler<TMessage> handler, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _handler = handler;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task HandleAsync(TMessage message, CancellationToken token)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return;

        var consumer = _handler.GetType().FullName;

        var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
        if (isMessageConsumed)
        {
            return;
        }
        
        var messageConsumerId = _identifierProvider.CreateNewId();
        var consumerMessage = ConsumedMessage.Create(messageConsumerId, messageId.Value, consumer, _dateTimeProvider.CurrentDateTime());
        _dbContext.Set<ConsumedMessage>().Add(consumerMessage);

        await _handler.HandleAsync(message, token);

        if (_dbContext.ChangeTracker.HasChanges())
            await _dbContext.SaveChangesAsync(token);
    }
}

    




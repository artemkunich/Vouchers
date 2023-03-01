using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Persistence;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Primitives;

namespace Vouchers.Infrastructure.InterCommunication;

public class MessageHandler<TEvent> : IMessageHandler<TEvent> where TEvent : IEvent
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IEnumerable<IEventPipelineBehavior<TEvent>> _pipelineBehaviors;
    private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public MessageHandler(IMessageHelper messageHelper, VouchersDbContext dbContext, IEnumerable<IEventPipelineBehavior<TEvent>> pipelineBehaviors, IEnumerable<IEventHandler<TEvent>> handlers, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _pipelineBehaviors = pipelineBehaviors;
        _handlers = handlers;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task HandleAsync(TEvent message, CancellationToken cancellation)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return;

        foreach (var handler in _handlers)
        {
            var consumer = handler.GetType().FullName;

            var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
            if (isMessageConsumed)
            {
                return;
            }

            var reversedBehaviors = _pipelineBehaviors.Reverse();

            Func<TEvent, CancellationToken, Task<Result<Unit>>> next = handler.HandleAsync;
            foreach (var behavior in reversedBehaviors)
            {
                var behaviorNext = next;
                next = (req, token) => behavior.HandleAsync(req, token, async () => await behaviorNext(req, token));
            }
            
            
            
            var consumedMessageId = _identifierProvider.CreateNewId();
            var consumedMessage = ConsumedMessage.Create(consumedMessageId, messageId.Value, consumer, _dateTimeProvider.CurrentDateTime());
            _dbContext.Set<ConsumedMessage>().Add(consumedMessage);

            var result = await next(message, cancellation);

            if (_dbContext.ChangeTracker.HasChanges())
                await _dbContext.SaveChangesAsync(cancellation);
        }
    }
}

    




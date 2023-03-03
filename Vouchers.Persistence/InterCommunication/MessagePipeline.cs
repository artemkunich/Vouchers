using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Persistence.InterCommunication.Errors;
using Vouchers.Primitives;

namespace Vouchers.Persistence.InterCommunication;

public class MessagePipeline<TEvent> : IMessagePipeline<TEvent> where TEvent : IEvent
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IEnumerable<IEventPipelineBehavior<TEvent>> _pipelineBehaviors;
    private readonly IEnumerable<IEventHandler<TEvent>> _handlers;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public MessagePipeline(IMessageHelper messageHelper, VouchersDbContext dbContext, IEnumerable<IEventPipelineBehavior<TEvent>> pipelineBehaviors, IEnumerable<IEventHandler<TEvent>> handlers, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _pipelineBehaviors = pipelineBehaviors.Reverse().ToArray();
        _handlers = handlers;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<Unit>> HandleAsync(TEvent message, CancellationToken cancellation)
    {
        var messageId = _messageHelper.GetMessageId(message);
        if (messageId is null)
            return new MessageWithoutIdError();

        foreach (var handler in _handlers)
        {
            var consumer = handler.GetType().FullName;

            var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId.Value, consumer);
            if (isMessageConsumed)
            {
                return Unit.Value;
            }

            var nextFunc = CreateNextFunc(handler); 
            
            var consumedMessageId = _identifierProvider.CreateNewId();
            var consumedMessage = ConsumedMessage.Create(consumedMessageId, messageId.Value, consumer, _dateTimeProvider.CurrentDateTime());
            _dbContext.Set<ConsumedMessage>().Add(consumedMessage);

            var result = await nextFunc(message, cancellation);

            if (result.IsFailure)
                return result;
                
            if (_dbContext.ChangeTracker.HasChanges())
                await _dbContext.SaveChangesAsync(cancellation);
            
        }

        return Unit.Value;
    }

    private Func<TEvent, CancellationToken, Task<Result<Unit>>> CreateNextFunc(IEventHandler<TEvent> handler)
    {
        var nextFunc = handler.HandleAsync;
        foreach (var behavior in _pipelineBehaviors)
        {
            var behaviorNext = nextFunc;
            nextFunc = (req, token) => behavior.HandleAsync(req, token, async () => await behaviorNext(req, token));
        }

        return nextFunc;
    }
}

    




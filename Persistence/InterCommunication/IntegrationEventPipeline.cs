using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Time;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Persistence.InterCommunication.Errors;

namespace Vouchers.Persistence.InterCommunication;

public class IntegrationEventPipeline<TEvent> : IIntegrationEventPipeline<TEvent> where TEvent : IIntegrationEvent
{
    private readonly IMessageHelper _messageHelper;
    private readonly VouchersDbContext _dbContext;
    private readonly IEnumerable<IIntegrationEventPipelineBehavior<TEvent>> _pipelineBehaviors;
    private readonly IEnumerable<IIntegrationEventHandler<TEvent>> _handlers;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _dateTimeProvider;
    
    public IntegrationEventPipeline(IMessageHelper messageHelper, VouchersDbContext dbContext, IEnumerable<IIntegrationEventPipelineBehavior<TEvent>> pipelineBehaviors, IEnumerable<IIntegrationEventHandler<TEvent>> handlers, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _messageHelper = messageHelper;
        _dbContext = dbContext;
        _pipelineBehaviors = pipelineBehaviors.Reverse().ToArray();
        _handlers = handlers;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation)
    {
        var messageId = @event.Id;

        foreach (var handler in _handlers)
        {
            var consumer = handler.GetType().FullName;

            var isMessageConsumed = await _messageHelper.CheckIfMessageWasConsumedAsync(messageId, consumer);
            if (isMessageConsumed)
            {
                return Unit.Value;
            }

            var nextFunc = CreateNextFunc(handler); 
            
            var consumedMessageId = _identifierProvider.CreateNewId();
            var consumedMessage = ConsumedMessage.Create(consumedMessageId, messageId, consumer, _dateTimeProvider.CurrentDateTime());
            _dbContext.Set<ConsumedMessage>().Add(consumedMessage);

            var result = await nextFunc(@event, cancellation);

            if (result.IsFailure)
                return result;
                
            if (_dbContext.ChangeTracker.HasChanges())
                await _dbContext.SaveChangesAsync(cancellation);
            
        }

        return Unit.Value;
    }

    private Func<TEvent, CancellationToken, Task<Result<Unit>>> CreateNextFunc(IIntegrationEventHandler<TEvent> handler)
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

    




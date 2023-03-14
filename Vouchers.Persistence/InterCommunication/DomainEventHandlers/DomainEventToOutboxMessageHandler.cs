using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Persistence.InterCommunication.DomainEventHandlers;

public class DomainEventToOutboxMessageHandler<TDomainEvent, TIntegrationEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IMessageDataSerializer _messageDataSerializer;
    private readonly IEventMapper<TDomainEvent, TIntegrationEvent> _eventMapper;
    private readonly VouchersDbContext _dbContext;
    
    public DomainEventToOutboxMessageHandler(IIdentifierProvider<Guid> identifierProvider, IMessageDataSerializer messageDataSerializer, VouchersDbContext dbContext, IEventMapper<TDomainEvent, TIntegrationEvent> eventMapper)
    {
        _identifierProvider = identifierProvider;
        _messageDataSerializer = messageDataSerializer;
        _dbContext = dbContext;
        _eventMapper = eventMapper;
    }

    public async Task<Result<Unit>> HandleAsync(TDomainEvent domainEvent, CancellationToken cancellation)
    {
        var outboxMessageId = _identifierProvider.CreateNewId();
        var integrationEvent = _eventMapper.Map(domainEvent);
        var data = await _messageDataSerializer.SerializeAsync(integrationEvent);
        var outboxMessage = OutboxMessage.Create(outboxMessageId, typeof(TIntegrationEvent).FullName, data);
        _dbContext.Set<OutboxMessage>().Add(outboxMessage);

        return Unit.Value;
    }
}
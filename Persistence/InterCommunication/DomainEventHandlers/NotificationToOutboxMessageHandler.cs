using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.Persistence.InterCommunication.DomainEventHandlers;

public class NotificationToOutboxMessageHandler<TDomainEvent, TIntegrationEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : INotification
{
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IMessageDataSerializer _messageDataSerializer;
    private readonly IEventMapper<TDomainEvent, TIntegrationEvent> _eventMapper;
    private readonly VouchersDbContext _dbContext;
    
    public NotificationToOutboxMessageHandler(IIdentifierProvider<Guid> identifierProvider, IMessageDataSerializer messageDataSerializer, VouchersDbContext dbContext, IEventMapper<TDomainEvent, TIntegrationEvent> eventMapper)
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
using System;
using System.Threading.Tasks;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.Application.ServiceProviders;

public class MessageFactory : IMessageFactory
{
    private readonly IMessageDataSerializer _messageDataSerializer;

    public MessageFactory(IMessageDataSerializer messageDataSerializer)
    {
        _messageDataSerializer = messageDataSerializer;
    }
    public async Task<OutboxMessage> CreateOutboxAsync(string type, object data)
    {
        return OutboxMessage.Create(type, await _messageDataSerializer.Serialize(data));
    }
    public async Task<OutboxMessage> CreateOutboxAsync(Event @event)
    {
        return OutboxMessage.Create(@event.GetType().Name, await _messageDataSerializer.Serialize(@event));
    }

    public async Task<InboxMessage> CreateInboxAsync(Guid originalId, string handler, object data)
    {
        return InboxMessage.Create(originalId, handler, await _messageDataSerializer.Serialize(data));
    }
}
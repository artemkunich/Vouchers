using System;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.Application.Services;

public interface IMessageFactory
{
    Task<OutboxMessage> CreateOutboxAsync(string type, object data);
    Task<OutboxMessage> CreateOutboxAsync(Event @event);
    
    Task<InboxMessage> CreateInboxAsync(Guid originalId, string handler, object data);
}
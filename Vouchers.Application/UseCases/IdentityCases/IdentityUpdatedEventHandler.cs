using System;
using System.Formats.Asn1;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.ServiceProviders;
using Vouchers.Application.Services;
using Vouchers.InterCommunication;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedEventHandler : IHandler<IdentityUpdatedEvent>
{
    private readonly IMessageFactory _messageFactory;
    private readonly IRepository<InboxMessage,Guid> _inboxMessageRepository;
    
    public IdentityUpdatedEventHandler(IMessageFactory messageFactory, IRepository<InboxMessage,Guid> inboxMessageRepository)
    {
        _messageFactory = messageFactory;
        _inboxMessageRepository = inboxMessageRepository;
    }
    
    public async Task HandleAsync(IdentityUpdatedEvent request, CancellationToken cancellation)
    {
        var inboxMessage = await _messageFactory.CreateInboxAsync(request.EventId, nameof(IdentityUpdatedEventHandler), request);
        inboxMessage.Process();
        await _inboxMessageRepository.AddAsync(inboxMessage);
    }
}
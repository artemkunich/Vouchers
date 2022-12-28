using System;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Entities;

namespace Vouchers.Application.ServiceProviders;

public class OutboxEventFactory : IOutboxEventFactory
{
    private readonly IEventDataSerializer _eventDataSerializer;

    public OutboxEventFactory(IEventDataSerializer eventDataSerializer)
    {
        _eventDataSerializer = eventDataSerializer;
    }
    
    public async Task<OutboxEvent> CreateAsync<TEvent,TEntity>(TEvent @event, Entity<TEntity> entity)
    {
        return OutboxEvent.Create(@event.GetType().Name, await _eventDataSerializer.Serialize(@event), entity);
    }
}
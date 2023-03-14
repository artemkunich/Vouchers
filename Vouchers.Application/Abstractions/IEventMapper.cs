using System.Data;

namespace Vouchers.Application.Abstractions;

public interface IEventMapper<in TDomainEvent, out TIntegrationEvent>
{
    TIntegrationEvent Map(TDomainEvent @event);
}
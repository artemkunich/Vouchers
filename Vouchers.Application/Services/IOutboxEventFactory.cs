using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Entities;

namespace Vouchers.Application.Services;

public interface IOutboxEventFactory
{
    Task<OutboxEvent> CreateAsync<TEvent, TEntity>(TEvent @event, Entity<TEntity> entity);
}
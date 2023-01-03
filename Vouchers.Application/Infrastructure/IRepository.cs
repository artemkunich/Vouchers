using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.Application.Infrastructure;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);

    Task UpdateAsync(TEntity entity, params OutboxMessage[] outboxMessages);
    Task UpdateAsync(TEntity entity, InboxMessage inboxMessages, params OutboxMessage[] outboxMessages);

    
    Task AddAsync(TEntity entity, params OutboxMessage[] outboxMessages);
    Task AddAsync(TEntity entity, InboxMessage inboxMessages, params OutboxMessage[] outboxMessages);

    Task RemoveAsync(TEntity entity, params OutboxMessage[] outboxMessages);
    Task RemoveAsync(TEntity entity, InboxMessage inboxMessages, params OutboxMessage[] outboxMessages);
}
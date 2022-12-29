using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.Application.Infrastructure
{
    public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        TEntity GetById(TKey id);

        Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetByExpression(Expression<Func<TEntity,bool>> expression);

        Task UpdateAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);
        void Update(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);

        Task AddAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);
        void Add(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);

        Task RemoveAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);
        void Remove(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null);
    }
}

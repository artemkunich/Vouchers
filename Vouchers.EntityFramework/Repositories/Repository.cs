using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.EntityFramework.Repositories
{
    internal abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity: Entity<TKey>
    {
        protected VouchersDbContext DbContext { get; }

        protected Repository(VouchersDbContext context)
        {
            DbContext = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            var dbSet = DbContext.Set<TEntity>();
            return await dbSet.FindAsync(id);
        }
     
        public virtual TEntity GetById(TKey id)
        {
            var dbSet = DbContext.Set<TEntity>();
            return dbSet.Find(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            var dbSet = DbContext.Set<TEntity>();
            return await dbSet.Where(expression).ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            var dbSet = DbContext.Set<TEntity>();
            return dbSet.Where(expression).ToList();
        }

        public virtual async Task UpdateAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Update(entity);

            await AddMessagesAsync(outboxMessages, inboxMessages);
                    
            await DbContext.SaveChangesAsync();
            
        }

        public virtual void Update(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Update(entity);
            
            AddMessages(outboxMessages, inboxMessages);
            
            DbContext.SaveChanges();
            
        }

        public virtual async Task AddAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            await dbSet.AddAsync(entity);
            
            await AddMessagesAsync(outboxMessages, inboxMessages);
            
            await DbContext.SaveChangesAsync();
            
        }

        public virtual void Add(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Add(entity);
            
            AddMessages(outboxMessages, inboxMessages);
            
            DbContext.SaveChanges();
            
        }

        public virtual async Task RemoveAsync(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Remove(entity);
            
            await AddMessagesAsync(outboxMessages, inboxMessages);
            
            await DbContext.SaveChangesAsync();
            
        }

        public virtual void Remove(TEntity entity, IEnumerable<OutboxMessage> outboxMessages = null, IEnumerable<InboxMessage> inboxMessages = null)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Remove(entity);
            
            AddMessages(outboxMessages, inboxMessages);
            
            DbContext.SaveChanges();
            
        }

        protected async Task AddMessagesAsync(IEnumerable<OutboxMessage> outboxMessages, IEnumerable<InboxMessage> inboxMessages)
        {
            if (outboxMessages != null && outboxMessages.Any())
                await DbContext.OutboxMessages.AddRangeAsync(outboxMessages);
            
            if (inboxMessages != null && inboxMessages.Any())
                await DbContext.InboxMessages.AddRangeAsync(inboxMessages);
        }
        
        protected void AddMessages(IEnumerable<OutboxMessage> outboxMessages, IEnumerable<InboxMessage> inboxMessages)
        {
            if (outboxMessages != null && outboxMessages.Any())
                DbContext.OutboxMessages.AddRange(outboxMessages);
            
            if (inboxMessages != null && inboxMessages.Any())
                DbContext.InboxMessages.AddRange(inboxMessages);
        }
    }
}

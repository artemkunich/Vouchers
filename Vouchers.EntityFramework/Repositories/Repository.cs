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

namespace Vouchers.EntityFramework.Repositories;

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

    public virtual async Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var dbSet = DbContext.Set<TEntity>();
        return await dbSet.Where(expression).ToListAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity, params OutboxMessage[] outboxMessages) =>
        await UpdateAsync(entity, null, outboxMessages);

    public virtual async Task UpdateAsync(TEntity entity, InboxMessage inboxMessage, params OutboxMessage[] outboxMessages)
    {
        var dbSet = DbContext.Set<TEntity>();
        
        dbSet.Update(entity);
        await AddMessagesAsync(outboxMessages, inboxMessage);
        
        await DbContext.SaveChangesAsync();
            
    }

    public virtual async Task AddAsync(TEntity entity, params OutboxMessage[] outboxMessages) =>
        await AddAsync(entity, null, outboxMessages);
    public virtual async Task AddAsync(TEntity entity, InboxMessage inboxMessage, params OutboxMessage[] outboxMessages)
    {
        var dbSet = DbContext.Set<TEntity>();
        
        await dbSet.AddAsync(entity);
        await AddMessagesAsync(outboxMessages, inboxMessage);
            
        await DbContext.SaveChangesAsync();
            
    }

    public virtual async Task RemoveAsync(TEntity entity, params OutboxMessage[] outboxMessages) =>
        await RemoveAsync(entity, null, outboxMessages);
    public virtual async Task RemoveAsync(TEntity entity, InboxMessage inboxMessage, params OutboxMessage[] outboxMessages)
    {
        var dbSet = DbContext.Set<TEntity>();
        
        dbSet.Remove(entity);
        await AddMessagesAsync(outboxMessages, inboxMessage);
            
        await DbContext.SaveChangesAsync();
            
    }

    protected async Task AddMessagesAsync(IEnumerable<OutboxMessage> outboxMessages, InboxMessage inboxMessage)
    {
        if (outboxMessages != null && outboxMessages.Any())
            await DbContext.OutboxMessages.AddRangeAsync(outboxMessages);
            
        if (inboxMessage != null)
            await DbContext.InboxMessages.AddAsync(inboxMessage);
    }
}
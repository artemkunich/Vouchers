using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vouchers.Application.Infrastructure;
using Vouchers.Primitives;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal abstract class Repository<TAggregateRoot, TKey> : ReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey> where TAggregateRoot: AggregateRoot<TKey>
{
    private readonly IMessageDataSerializer _messageDataSerializer;
    private readonly IIdentifierProvider<Guid> _identifierProvider;

    protected Repository(VouchersDbContext dbContext, IMessageDataSerializer messageDataSerializer, IIdentifierProvider<Guid> identifierProvider) : base(dbContext)
    {
        _messageDataSerializer = messageDataSerializer;
        _identifierProvider = identifierProvider;
    }

    // public virtual async Task<TAggregateRoot> GetByIdAsync(TKey id)
    // {
    //     var dbSet = DbContext.Set<TAggregateRoot>();
    //     return await dbSet.FindAsync(id);
    // }
    //
    // public virtual async Task<IEnumerable<TAggregateRoot>> GetByExpressionAsync(Expression<Func<TAggregateRoot, bool>> expression)
    // {
    //     var dbSet = DbContext.Set<TAggregateRoot>();
    //     return await dbSet.Where(expression).ToListAsync();
    // }

    public virtual async Task UpdateAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Update(aggregateRoot);
        await SaveChangesAsync();
            
    }
    
    public virtual async Task AddAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        await dbSet.AddAsync(aggregateRoot);
        await SaveChangesAsync();
            
    }
    
    public virtual async Task RemoveAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Remove(aggregateRoot);
        await SaveChangesAsync();
            
    }

    protected async Task SaveChangesAsync()
    {
        var domainEvents = DbContext.ChangeTracker.Entries<TAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents;
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessageId = _identifierProvider.CreateNewId();
            var outboxMessage = OutboxMessage.Create(outboxMessageId, domainEvent.GetType().FullName, await _messageDataSerializer.SerializeAsync(domainEvent));
            DbContext.Set<OutboxMessage>().Add(outboxMessage);
        }
        
        await DbContext.SaveChangesAsync();
    }
}
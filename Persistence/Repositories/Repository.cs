using System;
using System.Threading.Tasks;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Primitives;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal abstract class Repository<TAggregateRoot, TKey> : ReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey> where TAggregateRoot: AggregateRoot<TKey>
{
    protected Repository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public virtual Task UpdateAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Update(aggregateRoot);
        
        return Task.CompletedTask;
    }
    
    public virtual async Task AddAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        await dbSet.AddAsync(aggregateRoot);
    }
    
    public virtual Task RemoveAsync(TAggregateRoot aggregateRoot)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Remove(aggregateRoot);

        return Task.CompletedTask;
    }
}
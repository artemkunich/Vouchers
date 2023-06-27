using System.Threading;
using System.Threading.Tasks;
using Akunich.Domain.Abstractions;

namespace Vouchers.Persistence.Repositories;

internal abstract class Repository<TAggregateRoot, TKey> : ReadOnlyRepository<TAggregateRoot, TKey>, IRepository<TAggregateRoot, TKey> where TAggregateRoot: AggregateRoot<TKey>
{
    protected Repository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public virtual Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellation)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Update(aggregateRoot);
        
        return Task.CompletedTask;
    }
    
    public virtual async Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellation)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        await dbSet.AddAsync(aggregateRoot, cancellation);
    }
    
    public virtual Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellation)
    {
        var dbSet = DbContext.Set<TAggregateRoot>();
        dbSet.Remove(aggregateRoot);

        return Task.CompletedTask;
    }
}
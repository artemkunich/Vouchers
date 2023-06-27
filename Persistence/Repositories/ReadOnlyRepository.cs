using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Domain.Abstractions;

namespace Vouchers.Persistence.Repositories;

internal abstract class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
{
    protected VouchersDbContext DbContext { get; }

    protected ReadOnlyRepository(VouchersDbContext context)
    {
        DbContext = context;
    }

    public virtual async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellation)
    {
        var dbSet = DbContext.Set<TEntity>();
        return await dbSet.FindAsync(new object[] { id }, cancellationToken: cancellation);
    }

    public virtual async Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellation)
    {
        var dbSet = DbContext.Set<TEntity>();
        return await dbSet.Where(expression).ToListAsync(cancellation);
    }
}
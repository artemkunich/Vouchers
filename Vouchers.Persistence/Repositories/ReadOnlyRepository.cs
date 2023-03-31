using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Primitives;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal abstract class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
{
    protected VouchersDbContext DbContext { get; }

    protected ReadOnlyRepository(VouchersDbContext context)
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
}
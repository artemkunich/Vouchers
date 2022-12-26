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

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Update(entity);
            DbContext.SaveChanges();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            await dbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void Add(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void Remove(TEntity entity)
        {
            var dbSet = DbContext.Set<TEntity>();
            dbSet.Remove(entity);
            DbContext.SaveChanges();
        }
    }
}

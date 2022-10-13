using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Entities;

namespace Vouchers.EntityFramework.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        VouchersDbContext _dbContext;
        protected VouchersDbContext DbContext => _dbContext;

        protected Repository(VouchersDbContext context)
        {
            _dbContext = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.FirstOrDefaultAsync(entity => entity.Id == id);
        }
     
        public virtual TEntity GetById(Guid id)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return dbSet.FirstOrDefault(entity => entity.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return await dbSet.Where(expression).ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            var dbSet = _dbContext.Set<TEntity>();
            return dbSet.Where(expression).ToList();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            await dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void Add(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void Remove(TEntity entity)
        {
            var dbSet = _dbContext.Set<TEntity>();
            dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}

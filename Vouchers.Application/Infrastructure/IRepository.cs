using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Entities;

namespace Vouchers.Application.Infrastructure
{
    public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        TEntity GetById(TKey id);

        Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetByExpression(Expression<Func<TEntity,bool>> expression);

        Task UpdateAsync(TEntity entity);
        void Update(TEntity entity);

        Task AddAsync(TEntity entity);
        void Add(TEntity entity);

        Task RemoveAsync(TEntity entity);
        void Remove(TEntity entity);
    }
}

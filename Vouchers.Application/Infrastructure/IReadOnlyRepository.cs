using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Primitives;

namespace Vouchers.Application.Infrastructure;

public interface IReadOnlyRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
}
using System.Linq.Expressions;
using Vouchers.Primitives;

namespace Vouchers.Common.Application.Infrastructure;

public interface IReadOnlyRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
}
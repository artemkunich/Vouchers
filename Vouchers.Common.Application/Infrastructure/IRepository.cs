using System.Linq.Expressions;
using Vouchers.Primitives;

namespace Vouchers.Common.Application.Infrastructure;

public interface IRepository<TAggregateRoot, in TKey> where TAggregateRoot : AggregateRoot<TKey>
{
    Task<TAggregateRoot> GetByIdAsync(TKey id);

    Task<IEnumerable<TAggregateRoot>> GetByExpressionAsync(Expression<Func<TAggregateRoot, bool>> expression);

    Task UpdateAsync(TAggregateRoot entity);

    Task AddAsync(TAggregateRoot entity);

    Task RemoveAsync(TAggregateRoot entity);
}
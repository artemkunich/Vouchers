using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Primitives;

namespace Vouchers.Application.Infrastructure;

public interface IRepository<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TKey>
{
    Task<TAggregateRoot> GetByIdAsync(TKey id);

    Task<IEnumerable<TAggregateRoot>> GetByExpressionAsync(Expression<Func<TAggregateRoot, bool>> expression);

    Task UpdateAsync(TAggregateRoot entity);

    Task AddAsync(TAggregateRoot entity);

    Task RemoveAsync(TAggregateRoot entity);
}
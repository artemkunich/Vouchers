using Akunich.Domain.Abstractions;

namespace Vouchers.Persistence.Repositories;

internal class GenericReadOnlyRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
{
    public GenericReadOnlyRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }
}
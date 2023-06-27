using Akunich.Domain.Abstractions;

namespace Vouchers.Persistence.Repositories;

internal sealed class GenericRepository<TAggregateRoot, TKey> : Repository<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TKey>
{
    public GenericRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }
}
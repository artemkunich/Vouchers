using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Repositories;

internal sealed class DomainOffersPerIdentityCounterRepository : Repository<DomainOffersPerIdentityCounter, Guid>
{
    public DomainOffersPerIdentityCounterRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<DomainOffersPerIdentityCounter> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<DomainOffersPerIdentityCounter> GetByIdQueryable(Guid id) =>
        DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(domainContract => domainContract.Id == id);

    public override async Task<IEnumerable<DomainOffersPerIdentityCounter>> GetByExpressionAsync(Expression<Func<DomainOffersPerIdentityCounter, bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<DomainOffersPerIdentityCounter> GetByExpressionQueryable(Expression<Func<DomainOffersPerIdentityCounter, bool>> expression) =>
        DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(expression);
}
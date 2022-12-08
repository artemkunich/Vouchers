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

namespace Vouchers.EntityFramework.Repositories
{
    internal sealed class DomainOffersPerIdentityCounterRepository : Repository<DomainOffersPerIdentityCounter>
    {
        public DomainOffersPerIdentityCounterRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<DomainOffersPerIdentityCounter> GetByIdAsync(Guid id) => await DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(domainContract => domainContract.Id == id)
            .FirstOrDefaultAsync();

        public override DomainOffersPerIdentityCounter GetById(Guid id) => DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(domainContract => domainContract.Id == id)
            .FirstOrDefault();

        public override async Task<IEnumerable<DomainOffersPerIdentityCounter>> GetByExpressionAsync(Expression<Func<DomainOffersPerIdentityCounter, bool>> expression) => await DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(expression)
            .ToListAsync();

        public override IEnumerable<DomainOffersPerIdentityCounter> GetByExpression(Expression<Func<DomainOffersPerIdentityCounter, bool>> expression) => DbContext.DomainOffersPerIdentityCounters
            .Include(counter => counter.Offer)
            .Where(expression)
            .ToList();
    }
}

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
    internal sealed class DomainRepository : Repository<Domain>
    {
        public DomainRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Domain> GetByIdAsync(Guid id) => await DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(domain => domain.Id == id)
            .FirstOrDefaultAsync();

        public override Domain GetById(Guid id) => DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(domain => domain.Id == id)
            .FirstOrDefault();

        public override async Task<IEnumerable<Domain>> GetByExpressionAsync(Expression<Func<Domain,bool>> expression) => await DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(expression)
            .ToListAsync();

        public override IEnumerable<Domain> GetByExpression(Expression<Func<Domain, bool>> expression) => DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(expression)
            .ToList();

    }
}

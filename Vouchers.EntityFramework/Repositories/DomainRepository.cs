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

internal sealed class DomainRepository : Repository<Domain, Guid>
{
    public DomainRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Domain> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<Domain> GetByIdQueryable(Guid id) =>
        DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(domain => domain.Id == id);

    public override async Task<IEnumerable<Domain>> GetByExpressionAsync(Expression<Func<Domain,bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();
        

    private IQueryable<Domain> GetByExpressionQueryable(Expression<Func<Domain, bool>> expression) =>
        DbContext.Domains
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(expression);

}
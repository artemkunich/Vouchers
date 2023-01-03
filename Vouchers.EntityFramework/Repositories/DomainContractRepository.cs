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

internal sealed class DomainContractRepository : Repository<DomainContract, Guid>
{
    public DomainContractRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<DomainContract> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<DomainContract> GetByIdQueryable(Guid id) => DbContext.DomainContracts
        .Include(domainContract => domainContract.Offer)
        .Include(domainContract => domainContract.OffersPerIdentityCounter)
        .Where(domainContract => domainContract.Id == id);

    public override async Task<IEnumerable<DomainContract>> GetByExpressionAsync(Expression<Func<DomainContract, bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<DomainContract> GetByExpressionQueryable(Expression<Func<DomainContract, bool>> expression) => 
        DbContext.DomainContracts
            .Include(domainContract => domainContract.Offer)
            .Include(domainContract => domainContract.OffersPerIdentityCounter)
            .Where(expression);
}
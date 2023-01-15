using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class DomainRepository : Repository<Domain, Guid>
{
    public DomainRepository(VouchersDbContext dbContext, IMessageDataSerializer messageDataSerializer) : base(dbContext, messageDataSerializer)
    {
    }

    public override async Task<Domain> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<Domain> GetByIdQueryable(Guid id) =>
        DbContext.Set<Domain>()
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(domain => domain.Id == id);

    public override async Task<IEnumerable<Domain>> GetByExpressionAsync(Expression<Func<Domain,bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();
        

    private IQueryable<Domain> GetByExpressionQueryable(Expression<Func<Domain, bool>> expression) =>
        DbContext.Set<Domain>()
            .Include(domain => domain.Contract).ThenInclude(contract => contract.Offer)
            .Include(domain => domain.Contract).ThenInclude(contract => contract.OffersPerIdentityCounter)
            .Where(expression);

}
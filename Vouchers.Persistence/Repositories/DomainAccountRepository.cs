using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class DomainAccountRepository : Repository<DomainAccount, Guid>
{
    public DomainAccountRepository(VouchersDbContext dbContext, IMessageDataSerializer messageDataSerializer) : base(dbContext, messageDataSerializer)
    { 
    }

    public override async Task<DomainAccount> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<DomainAccount> GetByIdQueryable(Guid id) =>
        DbContext.Set<DomainAccount>()
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(acc => acc.Id == id);

    public override async Task<IEnumerable<DomainAccount>> GetByExpressionAsync(Expression<Func<DomainAccount,bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<DomainAccount> GetByExpressionQueryable(Expression<Func<DomainAccount, bool>> expression) =>
        DbContext.Set<DomainAccount>()
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(expression);
}
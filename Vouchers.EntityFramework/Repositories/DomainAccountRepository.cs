using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Repositories
{
    public sealed class DomainAccountRepository : Repository<DomainAccount>
    {
        public DomainAccountRepository(VouchersDbContext dbContext) : base(dbContext)
        { 
        }

        public override async Task<DomainAccount> GetByIdAsync(Guid id) => await DbContext.DomainAccounts
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(acc => acc.Id == id)
            .FirstOrDefaultAsync();

        public override DomainAccount GetById(Guid id) => DbContext.DomainAccounts
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(acc => acc.Id == id)
            .FirstOrDefault();

        public override async Task<IEnumerable<DomainAccount>> GetByExpressionAsync(Expression<Func<DomainAccount,bool>> expression) => await DbContext.DomainAccounts
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(expression)
            .ToListAsync();

        public override IEnumerable<DomainAccount> GetByExpression(Expression<Func<DomainAccount, bool>> expression) => DbContext.DomainAccounts
            .Include(acc => acc.Domain).ThenInclude(domain => domain.Contract)
            .Where(expression)
            .ToList();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    internal sealed class AccountItemRepository : Repository<AccountItem, Guid>
    {
        public AccountItemRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<AccountItem> GetByIdAsync(Guid id) => 
            await GetByIdQueryable(id).FirstOrDefaultAsync();

        public override AccountItem GetById(Guid id) =>
            GetByIdQueryable(id).FirstOrDefault();

        private IQueryable<AccountItem> GetByIdQueryable(Guid id) =>
            DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(account => account.Id == id);
        
        
        
        public override async Task<IEnumerable<AccountItem>> GetByExpressionAsync(Expression<Func<AccountItem,bool>> expression) =>
            await GetByExpressionQueryable(expression).ToListAsync();

        public override IEnumerable<AccountItem> GetByExpression(Expression<Func<AccountItem, bool>> expression) =>
            GetByExpressionQueryable(expression).ToList();
        
        private IQueryable<AccountItem> GetByExpressionQueryable(Expression<Func<AccountItem, bool>> expression) =>
            DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(expression);

    }
}

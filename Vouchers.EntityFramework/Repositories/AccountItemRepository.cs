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
    internal sealed class AccountItemRepository : Repository<AccountItem>
    {
        public AccountItemRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<AccountItem> GetByIdAsync(Guid id) => 
            await DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(account => account.Id == id).FirstOrDefaultAsync();

        public override AccountItem GetById(Guid id) =>
            DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(account => account.Id == id).FirstOrDefault();

        public override async Task<IEnumerable<AccountItem>> GetByExpressionAsync(Expression<Func<AccountItem,bool>> expression) =>
            await DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(expression).ToListAsync();

        public override IEnumerable<AccountItem> GetByExpression(Expression<Func<AccountItem, bool>> expression) =>
            DbContext.AccountItems
                .Include(account => account.HolderAccount)
                .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
                .Where(expression).ToList();

    }
}

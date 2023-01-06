using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class AccountItemRepository : Repository<AccountItem, Guid>
{
    public AccountItemRepository(VouchersDbContext dbContext, IMessageDataSerializer messageDataSerializer) : base(dbContext, messageDataSerializer)
    {
    }

    public override async Task<AccountItem> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();
        

    private IQueryable<AccountItem> GetByIdQueryable(Guid id) =>
        DbContext.Set<AccountItem>()
            .Include(account => account.HolderAccount)
            .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
            .Where(account => account.Id == id);
        
        
        
    public override async Task<IEnumerable<AccountItem>> GetByExpressionAsync(Expression<Func<AccountItem,bool>> expression) =>
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<AccountItem> GetByExpressionQueryable(Expression<Func<AccountItem, bool>> expression) =>
        DbContext.Set<AccountItem>()
            .Include(account => account.HolderAccount)
            .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
            .Where(expression);

}
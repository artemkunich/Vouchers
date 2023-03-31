using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Domain;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class UnitRepository : Repository<Unit, Guid>
{
    public UnitRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Unit> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();
        
    private IQueryable<Unit> GetByIdQueryable(Guid id) => 
        DbContext.Set<Unit>()
            .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
            .Where(unit => unit.Id == id);

        
    public override async Task<IEnumerable<Unit>> GetByExpressionAsync(Expression<Func<Unit,bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();
        
    private IQueryable<Unit> GetByExpressionQueryable(Expression<Func<Unit, bool>> expression) =>
        DbContext.Set<Unit>()
            .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
            .Where(expression); 
}
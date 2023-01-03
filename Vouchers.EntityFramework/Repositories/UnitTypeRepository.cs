using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories;

internal sealed class UnitTypeRepository : Repository<UnitType, Guid>
{
    public UnitTypeRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<UnitType> GetByIdAsync(Guid id) =>
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<UnitType> GetByIdQueryable(Guid id) =>
        DbContext.UnitTypes
            .Include(unitType => unitType.IssuerAccount)
            .Where(unitType => unitType.Id == id);
        
    public override async Task<IEnumerable<UnitType>> GetByExpressionAsync(Expression<Func<UnitType,bool>> expression) =>
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<UnitType> GetByExpressionQueryable(Expression<Func<UnitType, bool>> expression) =>
        DbContext.UnitTypes
            .Include(unitType => unitType.IssuerAccount)
            .Where(expression);

}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class UnitTypeRepository : Repository<UnitType, Guid>
{
    public UnitTypeRepository(VouchersDbContext dbContext, IMessageDataSerializer messageDataSerializer) : base(dbContext, messageDataSerializer)
    {
    }

    public override async Task<UnitType> GetByIdAsync(Guid id) =>
        await GetByIdQueryable(id).FirstOrDefaultAsync();

    private IQueryable<UnitType> GetByIdQueryable(Guid id) =>
        DbContext.Set<UnitType>()
            .Include(unitType => unitType.IssuerAccount)
            .Where(unitType => unitType.Id == id);
        
    public override async Task<IEnumerable<UnitType>> GetByExpressionAsync(Expression<Func<UnitType,bool>> expression) =>
        await GetByExpressionQueryable(expression).ToListAsync();

    private IQueryable<UnitType> GetByExpressionQueryable(Expression<Func<UnitType, bool>> expression) =>
        DbContext.Set<UnitType>()
            .Include(unitType => unitType.IssuerAccount)
            .Where(expression);

}
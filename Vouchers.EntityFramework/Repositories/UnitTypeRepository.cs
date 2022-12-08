using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    internal sealed class UnitTypeRepository : Repository<UnitType>
    {
        public UnitTypeRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<UnitType> GetByIdAsync(Guid id) =>
            await DbContext.UnitTypes
                .Include(unitType => unitType.IssuerAccount)
                .FirstOrDefaultAsync(unitType => unitType.Id == id);

        public override UnitType GetById(Guid id) =>
            DbContext.UnitTypes
                .Include(unitType => unitType.IssuerAccount)
                .FirstOrDefault(unitType => unitType.Id == id);

        public override async Task<IEnumerable<UnitType>> GetByExpressionAsync(Expression<Func<UnitType,bool>> expression) =>
            await DbContext.UnitTypes
                .Include(unitType => unitType.IssuerAccount)
                .Where(expression).ToListAsync();

        public override IEnumerable<UnitType> GetByExpression(Expression<Func<UnitType, bool>> expression) =>
            DbContext.UnitTypes
                .Include(unitType => unitType.IssuerAccount)
                .Where(expression).ToList();

    }
}

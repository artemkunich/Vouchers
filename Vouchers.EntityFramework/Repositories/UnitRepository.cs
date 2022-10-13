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
    public sealed class UnitRepository : Repository<Unit>
    {

        public UnitRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Unit> GetByIdAsync(Guid id) => await DbContext.Units
                .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                .FirstOrDefaultAsync(unit => unit.Id == id);

        public override Unit GetById(Guid id) => DbContext.Units
                .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                .FirstOrDefault(unit => unit.Id == id);

        public override async Task<IEnumerable<Unit>> GetByExpressionAsync(Expression<Func<Unit,bool>> expression) => await DbContext.Units
                .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                .Where(expression).ToListAsync();

        public override IEnumerable<Unit> GetByExpression(Expression<Func<Unit, bool>> expression) => DbContext.Units
                .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                .Where(expression).ToList();

    }
}

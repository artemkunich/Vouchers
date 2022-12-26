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
    internal sealed class UnitRepository : Repository<Unit, Guid>
    {

        public UnitRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Unit> GetByIdAsync(Guid id) => 
                await GetByIdQueryable(id).FirstOrDefaultAsync();

        public override Unit GetById(Guid id) => 
                GetByIdQueryable(id).FirstOrDefault();

        private IQueryable<Unit> GetByIdQueryable(Guid id) =>
                DbContext.Units
                        .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                        .Where(unit => unit.Id == id);

        public override async Task<IEnumerable<Unit>> GetByExpressionAsync(Expression<Func<Unit,bool>> expression) => 
                await GetByExpressionQueryable(expression).ToListAsync();

        public override IEnumerable<Unit> GetByExpression(Expression<Func<Unit, bool>> expression) =>
                GetByExpressionQueryable(expression).ToList();

        private IQueryable<Unit> GetByExpressionQueryable(Expression<Func<Unit, bool>> expression) =>
                DbContext.Units
                        .Include(unit => unit.UnitType).ThenInclude(unitType => unitType.IssuerAccount)
                        .Where(expression);

    }
}

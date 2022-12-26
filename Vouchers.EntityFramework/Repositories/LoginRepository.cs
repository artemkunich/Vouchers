using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Repositories
{
    internal sealed class LoginRepository : Repository<Login, Guid>
    {
        public LoginRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Login> GetByIdAsync(Guid id) => 
            await GetByIdQueryable(id).FirstOrDefaultAsync();

        public override Login GetById(Guid id) => 
            GetByIdQueryable(id).FirstOrDefault();

        private IQueryable<Login> GetByIdQueryable(Guid id) =>
            DbContext.Logins
                .Include(login => login.Identity)
                .Where(login => login.Id == id);
            
        public override async Task<IEnumerable<Login>> GetByExpressionAsync(Expression<Func<Login,bool>> expression) => 
            await GetByExpressionQueryable(expression).ToListAsync();

        public override IEnumerable<Login> GetByExpression(Expression<Func<Login, bool>> expression) => 
            GetByExpressionQueryable(expression).ToList();
        
        private IQueryable<Login> GetByExpressionQueryable(Expression<Func<Login, bool>> expression) =>
            DbContext.Logins
                .Include(login => login.Identity)
                .Where(expression);

    }
}

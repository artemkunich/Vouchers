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
    public class LoginRepository : Repository<Login>
    {
        public LoginRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Login> GetByIdAsync(Guid id) => await DbContext.Logins
            .Include(login => login.Identity)
            .Where(login => login.Id == id).FirstOrDefaultAsync();

        public override Login GetById(Guid id) => DbContext.Logins
            .Include(login => login.Identity)
            .Where(login => login.Id == id).FirstOrDefault();

        public override async Task<IEnumerable<Login>> GetByExpressionAsync(Expression<Func<Login,bool>> expression) => await DbContext.Logins
            .Include(login => login.Identity)
            .Where(expression).ToListAsync();

        public override IEnumerable<Login> GetByExpression(Expression<Func<Login, bool>> expression) => DbContext.Logins
            .Include(login => login.Identity)
            .Where(expression).ToList();

    }
}

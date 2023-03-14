using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core.Domain;
using Vouchers.Identities.Domain;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal sealed class LoginRepository : Repository<Login, Guid>
{
    public LoginRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Login> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();
        
    private IQueryable<Login> GetByIdQueryable(Guid id) =>
        DbContext.Set<Login>()
            .Include(login => login.Identity)
            .Where(login => login.Id == id);
            
        
    public override async Task<IEnumerable<Login>> GetByExpressionAsync(Expression<Func<Login,bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();
        
    private IQueryable<Login> GetByExpressionQueryable(Expression<Func<Login, bool>> expression) =>
        DbContext.Set<Login>()
            .Include(login => login.Identity)
            .Where(expression);

}
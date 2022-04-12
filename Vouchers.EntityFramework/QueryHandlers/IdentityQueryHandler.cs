using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.UseCases;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class IdentityQueryHandler : IHandler<string, Guid?>
    {
        VouchersDbContext dbContext;

        public IdentityQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Guid?> HandleAsync(string loginName, CancellationToken cancellation)
        {
            var login = await dbContext.Logins.Include(login => login.Identity).Where(l => l.LoginName == loginName).FirstOrDefaultAsync();

            return login?.Identity.Id;
        }
    }
}

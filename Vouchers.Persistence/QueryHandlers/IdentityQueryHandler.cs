using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Dtos;
using Vouchers.Application.UseCases;
using Vouchers.Identities.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IdentityQueryHandler : IHandler<string, IdDto<Guid?>>
{
    VouchersDbContext _dbContext;

    public IdentityQueryHandler(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IdDto<Guid?>>> HandleAsync(string loginName, CancellationToken cancellation)
    {
        var login = await _dbContext.Set<Login>().Include(login => login.Identity).Where(l => l.LoginName == loginName).FirstOrDefaultAsync(cancellation);

        return new IdDto<Guid?>(login?.Identity.Id);
    }
}
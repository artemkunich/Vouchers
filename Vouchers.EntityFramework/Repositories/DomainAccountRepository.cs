using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    public class DomainAccountRepository : IDomainAccountRepository
    {
        VouchersDbContext dbContext;

        public DomainAccountRepository(VouchersDbContext dbContext) =>
            this.dbContext = dbContext;

        public async Task<DomainAccount> GetByDomainIdAndIdentityIdAsync(Guid domainId, Guid identityId) =>
            throw new NotImplementedException();

        public DomainAccount GetByDomainIdAndIdentityId(Guid domainId, Guid identityId) =>
            throw new NotImplementedException();

        public DomainAccount GetById(Guid id) => dbContext.DomainAccounts
            .Where(user => user.Id == id)
            .FirstOrDefault();

        public async Task<DomainAccount> GetByIdAsync(Guid id) => await dbContext.DomainAccounts
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();

    }
}

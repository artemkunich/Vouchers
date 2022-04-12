using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Repositories
{
    public class IdentityDetailRepository : IIdentityDetailRepository
    {
        private readonly VouchersDbContext dbContext;

        public IdentityDetailRepository(VouchersDbContext dbContext) =>
            this.dbContext = dbContext;

        public async Task<IdentityDetail> GetByIdentityIdAsync(Guid identityId) =>
            await dbContext.IdentityDetails
                .Include(id => id.Identity)
                .Where(id => id.Identity.Id == identityId).FirstOrDefaultAsync();

        public IdentityDetail GetByIdentityId(Guid identityId) =>
            dbContext.IdentityDetails
                .Include(id => id.Identity)
                .Where(id => id.Identity.Id == identityId).FirstOrDefault();


        public void Update(IdentityDetail identityDetail) =>
            dbContext.IdentityDetails.Update(identityDetail);

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();
    }
}

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
    public class VoucherAccountRepository : IVoucherAccountRepository
    {
        private readonly VouchersDbContext dbContext;

        public VoucherAccountRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<VoucherAccount>> GetAsync(Guid userAccountId) => 
            await dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == userAccountId).ToListAsync();

        public IEnumerable<VoucherAccount> Get(Guid userAccountId) => 
            dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == userAccountId);

        public async Task<VoucherAccount> GetAsync(Guid userAccountId, Guid voucherId) =>
            await dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .FirstOrDefaultAsync(account => account.Holder.Id == userAccountId && account.Unit.Id == voucherId);


        public VoucherAccount Get(Guid userAccountId, Guid voucherId) =>
            dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .FirstOrDefault(account => account.Holder.Id == userAccountId && account.Unit.Id == voucherId);

        /*public IEnumerable<Account> Get(string userAccountId, int valueId)
        {
            return dbContext.Accounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == userAccountId && account.Unit.Value.Id == valueId);

        }*/
    }
}

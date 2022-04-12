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
    public class VoucherValueRepository : IVoucherValueRepository
    {
        private readonly VouchersDbContext dbContext;

        public VoucherValueRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VoucherValue> GetByIdAsync(Guid voucherId) =>
            await dbContext.VoucherValues
                .Include(value => value.Issuer)
                .FirstOrDefaultAsync(value => value.Id == voucherId);

        public VoucherValue GetById(Guid voucherId) =>          
            dbContext.VoucherValues
                .Include(value=>value.Issuer)
                .FirstOrDefault(value=>value.Id == voucherId);

        public void Update(VoucherValue value) =>
            throw new NotImplementedException();

        public void Remove(VoucherValue value) =>
            throw new NotImplementedException();

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();

    }
}

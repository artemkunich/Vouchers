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
    public class VoucherRepository : IVoucherRepository
    {
        private readonly VouchersDbContext dbContext;

        public VoucherRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Voucher> GetByIdAsync(Guid voucherId) =>
            await dbContext.Vouchers
                .Include(voucher => voucher.Value).ThenInclude(value => value.Issuer)
                .FirstOrDefaultAsync(voucher => voucher.Id == voucherId);

        public Voucher GetById(Guid voucherId) =>
            dbContext.Vouchers
                .Include(voucher=>voucher.Value).ThenInclude(value=>value.Issuer)                
                .FirstOrDefault(voucher=>voucher.Id == voucherId);

        public async Task AddAsync(Voucher voucher) =>
            await dbContext.Vouchers.AddAsync(voucher);

        public void Add(Voucher voucher) =>
            dbContext.Vouchers.Add(voucher);

        public void Update(Voucher voucher) =>
            dbContext.Vouchers.Update(voucher);

        public void Remove(Voucher voucher) =>
            dbContext.Vouchers.Remove(voucher);

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();
    }
}

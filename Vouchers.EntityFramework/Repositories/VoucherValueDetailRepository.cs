using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Values;

namespace Vouchers.EntityFramework.Repositories
{

    public class VoucherValueDetailRepository : IVoucherValueDetailRepository
    {
        VouchersDbContext dbContext;

        public VoucherValueDetailRepository(VouchersDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<VoucherValueDetail> GetByValueIdAsync(Guid valueId) =>
            await dbContext.VoucherValueDetails
                .Include(detail => detail.Value).ThenInclude(value => value.Issuer)
                .FirstOrDefaultAsync(detail => detail.Value.Id == valueId);

        public VoucherValueDetail GetByValueId(Guid valueId) =>
            dbContext.VoucherValueDetails
                .Include(detail => detail.Value).ThenInclude(value => value.Issuer)
                .FirstOrDefault(detail => detail.Value.Id == valueId);

        public async Task AddAsync(VoucherValueDetail valueDetail) =>
            await dbContext.AddAsync(valueDetail);

        public void Add(VoucherValueDetail valueDetail) =>
            dbContext.Add(valueDetail);

        public void Update(VoucherValueDetail valueDetail) =>
            dbContext.Update(valueDetail);

        public void Remove(VoucherValueDetail valueDetail) =>
            dbContext.Remove(valueDetail);

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();    
    }
}

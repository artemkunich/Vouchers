using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    public class HolderTransactionRepository : IHolderTransactionRepository
    {
        private readonly VouchersDbContext dbContext;

        public HolderTransactionRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(HolderTransaction transaction) =>
            await dbContext.HolderTransactions.AddAsync(transaction);

        public void Add(HolderTransaction transaction) =>          
            dbContext.HolderTransactions.AddAsync(transaction);

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Save() {
            dbContext.SaveChanges();
        }
    }
}

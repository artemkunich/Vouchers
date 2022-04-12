using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    public class IssuerTransactionRepository : IIssuerTransactionRepository
    {

        private readonly VouchersDbContext dbContext;

        public IssuerTransactionRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(IssuerTransaction transaction) =>
            await dbContext.IssuerTransactions.AddAsync(transaction);

        public void Add(IssuerTransaction transaction) =>
            dbContext.IssuerTransactions.Add(transaction);

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();
    }
}

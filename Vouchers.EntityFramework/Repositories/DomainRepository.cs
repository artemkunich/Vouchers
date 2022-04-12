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
    public class DomainRepository : IDomainRepository
    {
        VouchersDbContext dbContext;

        public DomainRepository(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Domain> GetByIdAsync(Guid id) => await dbContext.Domains
            .Where(domain => domain.Id == id)
            .FirstOrDefaultAsync();

        public Domain GetById(Guid id) => dbContext.Domains
            .Where(domain => domain.Id == id)
            .FirstOrDefault();

        public void Update(Domain domain)
        {
            throw new NotImplementedException();
        }


        public void Remove(Domain domain)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }      
    }
}

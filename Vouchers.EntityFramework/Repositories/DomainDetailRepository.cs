using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Repositories
{
    public class DomainDetailRepository : IDomainDetailRepository
    {
        VouchersDbContext dbContext;

        public DomainDetailRepository(VouchersDbContext dbContext) =>
            this.dbContext = dbContext;

        public async Task<DomainDetail> GetByDomainIdAsync(Guid id) => await dbContext.DomainDetails
            .Include(domainDetail => domainDetail.Contract).ThenInclude(contract => contract.Domain)
            .Where(domainDetail => domainDetail.Contract.Domain.Id == id)
            .FirstOrDefaultAsync();

        public DomainDetail GetByDomainId(Guid id) => dbContext.DomainDetails
            .Include(domainDetail => domainDetail.Contract.Domain)
            .Where(domainDetail => domainDetail.Contract.Domain.Id == id)
            .FirstOrDefault();

        public async Task AddAsync(DomainDetail domainDetail) => await dbContext.DomainDetails
            .AddAsync(domainDetail);

        public void Add(DomainDetail domainDetail) =>
            dbContext.DomainDetails.Add(domainDetail);

        public void Update(DomainDetail domainDetail) =>
            dbContext.DomainDetails.Update(domainDetail);

        public void Remove(DomainDetail domainDetail) =>
            throw new NotImplementedException();

        public async Task SaveAsync() =>
            await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();     
    }
}

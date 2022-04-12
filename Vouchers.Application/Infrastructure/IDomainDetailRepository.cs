using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Domains;

namespace Vouchers.Application.Infrastructure
{
    public interface IDomainDetailRepository
    {
        Task<DomainDetail> GetByDomainIdAsync(Guid domainId);
        DomainDetail GetByDomainId(Guid domainId);

        Task AddAsync(DomainDetail detail);
        void Add(DomainDetail detail);

        void Update(DomainDetail detail);

        void Remove(DomainDetail detail);

        Task SaveAsync();
        void Save();
    }
}

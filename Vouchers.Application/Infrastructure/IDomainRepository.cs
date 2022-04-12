using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IDomainRepository
    {
        Task<Domain> GetByIdAsync(Guid domainId);
        Domain GetById(Guid domainId);

        void Update(Domain domain);

        void Remove(Domain domain);

        Task SaveAsync();
        void Save();
    }
}

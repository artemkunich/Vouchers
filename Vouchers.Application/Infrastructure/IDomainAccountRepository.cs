using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IDomainAccountRepository
    {
        Task<DomainAccount> GetByIdAsync(Guid domainAccountid);
        DomainAccount GetById(Guid domainAccountid);

        Task<DomainAccount> GetByDomainIdAndIdentityIdAsync(Guid domainId, Guid identityId);
        DomainAccount GetByDomainIdAndIdentityId(Guid domainId, Guid identityId);
    }
}

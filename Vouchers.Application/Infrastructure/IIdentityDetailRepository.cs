using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.Application.Infrastructure
{
    public interface IIdentityDetailRepository
    {
        Task<IdentityDetail> GetByIdentityIdAsync(Guid identityId);
        IdentityDetail GetByIdentityId(Guid identityId);

        void Update(IdentityDetail identityDetail);

        Task SaveAsync();
        void Save();
    }
}

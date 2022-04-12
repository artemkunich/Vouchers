using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IVoucherAccountRepository
    {
        Task<VoucherAccount> GetAsync(Guid userAccountId, Guid voucherId);
        VoucherAccount Get(Guid userAccountId, Guid voucherId);
    }
}

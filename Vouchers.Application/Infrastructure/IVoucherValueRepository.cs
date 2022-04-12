using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IVoucherValueRepository
    {
        Task<VoucherValue> GetByIdAsync(Guid valueId);
        VoucherValue GetById(Guid valueId);

        void Update(VoucherValue value);

        void Remove(VoucherValue value);

        Task SaveAsync();
        void Save();
    }
}

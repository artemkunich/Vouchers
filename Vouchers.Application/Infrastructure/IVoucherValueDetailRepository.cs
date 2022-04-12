using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Values;

namespace Vouchers.Application.Infrastructure
{
    public interface IVoucherValueDetailRepository
    {
        Task<VoucherValueDetail> GetByValueIdAsync(Guid valueId);
        VoucherValueDetail GetByValueId(Guid valueId);

        Task AddAsync(VoucherValueDetail valueDetail);
        void Add(VoucherValueDetail valueDetail);

        void Update(VoucherValueDetail valueDetail);

        void Remove(VoucherValueDetail valueDetail);

        Task SaveAsync();
        void Save();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IVoucherRepository
    {
        Task<Voucher> GetByIdAsync(Guid voucherId);
        Voucher GetById(Guid voucherId);

        Task AddAsync(Voucher voucher);
        void Add(Voucher voucher);

        void Update(Voucher voucher);

        void Remove(Voucher voucher);

        Task SaveAsync();
        void Save();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IHolderTransactionRepository
    {
        Task AddAsync(HolderTransaction transaction);
        void Add(HolderTransaction transaction);

        Task SaveAsync();
        void Save();
    }
}

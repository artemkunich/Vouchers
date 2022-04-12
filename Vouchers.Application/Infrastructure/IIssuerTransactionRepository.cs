using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;

namespace Vouchers.Application.Infrastructure
{
    public interface IIssuerTransactionRepository
    {
        Task AddAsync(IssuerTransaction transaction);
        void Add(IssuerTransaction transaction);

        Task SaveAsync();
        void Save();
    }
}

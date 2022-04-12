using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.Application.Infrastructure
{
    public interface ILoginRepository
    {
        Task<Login> GetByLoginNameAsync(string loginName);
        Login GetByLoginName(string loginName);

        Task AddAsync(Login login, IdentityDetail identityDetail);
        void Add(Login login, IdentityDetail identityDetail);   
        
        void Update(Login login);
        void Remove(Login login);

        Task SaveAsync();
        void Save();
    }
}

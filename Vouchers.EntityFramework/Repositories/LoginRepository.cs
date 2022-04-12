using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly VouchersDbContext dbContext;

        public LoginRepository(VouchersDbContext dbContext) =>
            this.dbContext = dbContext;

        public async Task AddAsync(Login login, IdentityDetail identityDetail)
        {
            await dbContext.Logins.AddAsync(login);
            await dbContext.IdentityDetails.AddAsync(identityDetail);
        }

        public void Add(Login login, IdentityDetail identityDetail)
        {
            dbContext.Logins.Add(login);
            dbContext.IdentityDetails.Add(identityDetail);
        }

        public async Task<Login> GetByLoginNameAsync(string loginName) =>
            await dbContext.Logins.Where(login => login.LoginName == loginName).Include(login => login.Identity).FirstOrDefaultAsync();

        public Login GetByLoginName(string loginName) =>
            dbContext.Logins.Where(login => login.LoginName == loginName).Include(login=>login.Identity).FirstOrDefault();

        public void Update(Login login) =>
            dbContext.Logins.Update(login);

        public void Remove(Login login) =>
            dbContext.Logins.Remove(login);

        public async Task SaveAsync() =>
           await dbContext.SaveChangesAsync();

        public void Save() =>
            dbContext.SaveChanges();
    }
}

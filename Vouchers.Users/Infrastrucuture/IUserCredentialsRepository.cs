using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public interface IUserCredentialsRepository
    {
        bool ContainsCredentialsForNickname(string domain, string nickname);
        bool ContainsCredentialsForEmail(string domain, string nickname);

        UserCredentials GetByUserId(int id);
        UserCredentials GetByUserUId(string uId, string nickname);
        UserCredentials GetByEmail(string domain, string email);

        void Add(UserCredentials credentials);

        void Update(UserCredentials credentials);

        void Remove(UserCredentials credentials);

        void Save();
    }
}

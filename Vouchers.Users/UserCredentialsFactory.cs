using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Accounting;
using Vouchers.Application;

namespace Vouchers.Auth
{
    public class UserCredentialsFactory
    {
        public UserCredentials CreateUserCredentials(User user, string passHash)
        {
            return new UserCredentials(user, passHash, DateTime.Now);
        }

    }
}

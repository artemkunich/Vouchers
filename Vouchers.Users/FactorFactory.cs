using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public class FactorFactory
    {
        public AuthUserFactor CreateAuthUserFactor(UserCredentials userAccount, string code)
        {
            return new AuthUserFactor(userAccount, code);
        }

        public UpdateEmailFactor CreateUpdateEmailFactor(int userId, string email, string code)
        {
            return new UpdateEmailFactor(userId, email, code);
        }

        public UpdatePasswordFactor CreateUpdatePasswordFactor(int userId, string passwordHash, string code) {
            return new UpdatePasswordFactor(userId, passwordHash, code);
        }
    }
}

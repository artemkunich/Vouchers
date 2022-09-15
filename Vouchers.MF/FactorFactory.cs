using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.MF;
using Vouchers.Auth;

namespace Vouchers.MF
{
    public class FactorFactory
    {
        public AuthUserFactor CreateAuthUserFactor(UserCredentials userAccount, int code)
        {
            return new AuthUserFactor(userAccount, code);
        }

        public UpdateEmailFactor CreateUpdateEmailFactor(int userId, string email, int code)
        {
            return new UpdateEmailFactor(userId, email, code);
        }

        public UpdatePasswordFactor CreateUpdatePasswordFactor(int userId, string passwordHash, int code) {
            return new UpdatePasswordFactor(userId, passwordHash, code);
        }
    }
}

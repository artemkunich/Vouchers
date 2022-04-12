using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public interface IAuthUserFactors
    {
        AuthUserFactor Get(string domain, string nickname);
        void Save(AuthUserFactor authUserFactor);
    }
}

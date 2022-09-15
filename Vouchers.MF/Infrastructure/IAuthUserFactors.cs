using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.MF
{
    public interface IAuthUserFactors
    {
        AuthUserFactor Get(int accDomainId, string email);
        void Save(AuthUserFactor authUserFactor);
    }
}

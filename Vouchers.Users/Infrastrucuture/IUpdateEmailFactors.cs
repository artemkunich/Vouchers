using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public interface IUpdateEmailFactors
    {
        UpdateEmailFactor Get(int userAccountId);
        void Save(UpdateEmailFactor emailFactor);
    }
}

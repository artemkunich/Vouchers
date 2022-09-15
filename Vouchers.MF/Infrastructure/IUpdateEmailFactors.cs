using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.MF
{
    public interface IUpdateEmailFactors
    {
        UpdateEmailFactor Get(int userAccountId);
        void Save(UpdateEmailFactor emailFactor);
    }
}

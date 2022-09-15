using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.MF
{
    public interface IUpdatePasswordFactors
    {
        UpdatePasswordFactor Get(int userAccountId);
        void Save(UpdatePasswordFactor updatePasswordFactor);
    }
}

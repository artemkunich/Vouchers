using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Auth;

namespace Vouchers.MF
{
    public interface IMFCodeGenerator
    {
        int GenerateAndSendCodeToUser(UserCredentials authUser);
        int GenerateAndSendCodeToEmail(string email);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Auth
{
    public interface IMFCodeGenerator
    {
        string GenerateCode();
        string SendCodeToEmail(string email);
    }
}

using System;

namespace Vouchers.Auth
{
    public class AuthException : Exception
    {
        internal AuthException(string message) : base(message)
        {
        }
    }
}

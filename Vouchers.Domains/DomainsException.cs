using System;

namespace Vouchers.Domains
{
    public class DomainsException : Exception
    {
        internal DomainsException(string message) : base(message)
        {
        }
    }
}

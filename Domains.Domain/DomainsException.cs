using System;

namespace Vouchers.Domains.Domain;

public class DomainsException : Exception
{
    internal DomainsException(string message) : base(message)
    {
    }
}


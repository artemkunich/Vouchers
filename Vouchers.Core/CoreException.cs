using System;

namespace Vouchers.Core
{
    public class CoreException : Exception
    {
        internal CoreException(string message) : base(message)
        {
        }
    }
}

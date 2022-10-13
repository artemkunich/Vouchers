using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application
{
    public class ApplicationException : Exception
    {
        internal ApplicationException(string message) : base(message)
        {
        }
    }
}

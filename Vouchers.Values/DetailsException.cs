using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Details
{
    public class DetailsException : Exception
    {
        internal DetailsException(string message) : base(message)
        {
        }
    }
}

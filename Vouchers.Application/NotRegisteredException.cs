using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application;

public class NotRegisteredException : ApplicationException
{
    internal NotRegisteredException() : base("User is not registered")
    {
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Infrastructure;

public interface ILoginNameProvider
{
    string CurrentLoginName { get; }
}
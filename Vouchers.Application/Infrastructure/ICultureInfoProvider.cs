using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Infrastructure;

public interface ICultureInfoProvider
{
    CultureInfo GetCultureInfo();
}
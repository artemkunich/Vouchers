using System;
using System.Globalization;

namespace Vouchers.Core
{
    public class CoreException : Exception
    {
        internal CoreException(string resourceKey, CultureInfo cultureInfo) : base(CoreResources.GetString(resourceKey, cultureInfo))
        {
        }

        internal CoreException(string resourceKey, CultureInfo cultureInfo, params object[] args) : base(CoreResources.GetString(resourceKey, cultureInfo, args))
        {
        }
    }
}

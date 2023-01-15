using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Globalization;
using System.Reflection;

namespace Vouchers.Core.Domain;

internal class CoreResources
{
    private static ResourceManager _rm = new ResourceManager(typeof(Properties.Resources));

    public static string GetString(string resourceKey, CultureInfo cultureInfo) => _rm.GetString(resourceKey, cultureInfo);

    public static string GetString(string resourceKey, CultureInfo cultureInfo, params object[] args) => string.Format(_rm.GetString(resourceKey, cultureInfo), args);
}

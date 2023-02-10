using System.Globalization;
using System.Resources;

namespace Vouchers.Application.UseCases;

internal class ApplicationResources
{
    private static readonly ResourceManager _rm = new(typeof(Properties.Resources));

    public static string GetString(string resourceKey, CultureInfo cultureInfo) => _rm.GetString(resourceKey, cultureInfo);

    public static string GetString(string resourceKey, CultureInfo cultureInfo, params object[] args) => string.Format(_rm.GetString(resourceKey, cultureInfo), args);
}
using System.Globalization;
using System.Resources;
using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.Resources;

public class ResourceProvider : IResourceProvider
{
    private static readonly ResourceManager Rm = new(typeof(Properties.Resources));

    private readonly CultureInfo _cultureInfo;

    public ResourceProvider(ICultureInfoProvider cultureInfoProvider)
    {
        _cultureInfo = cultureInfoProvider.GetCultureInfo();
    }

    public string GetString(string resourceKey) => Rm.GetString(resourceKey, _cultureInfo) ?? resourceKey;

    public string GetString(string resourceKey, params object[] args)
    {
        var resource = Rm.GetString(resourceKey, _cultureInfo);
        if (resource is null)
            return resourceKey;
        
        return string.Format(resource, args);
    } 
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Resources;

namespace Vouchers.MinimalAPI.Services;

public class CultureInfoProvider : ICultureInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CultureInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CultureInfo GetCultureInfo() {
        var rqf = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
        return rqf?.RequestCulture.UICulture;
    }
}
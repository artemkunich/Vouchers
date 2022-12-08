﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Vouchers.Application.Infrastructure;

namespace Vouchers.API.Services
{
    public class CultureInfoProvider : ICultureInfoProvider
    {
        IHttpContextAccessor _httpContextAccessor;

        public CultureInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CultureInfo GetCultureInfo() {
            var rqf = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            return rqf.RequestCulture.UICulture;
        }
    }
}

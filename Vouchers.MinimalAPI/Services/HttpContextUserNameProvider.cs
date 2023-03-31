using Microsoft.AspNetCore.Http;
using System.Linq;
using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.MinimalAPI.Services;

public sealed class HttpContextUserNameProvider : ILoginNameProvider
{
    IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserNameProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Identity.Name; }
}
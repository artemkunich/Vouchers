using Microsoft.AspNetCore.Http;

namespace Vouchers.API.Services;

public sealed class HttpContextUserNameProvider : ILoginNameProvider
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserNameProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Identity.Name; }
}
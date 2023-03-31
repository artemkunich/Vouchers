using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.MinimalAPI.Services;

public sealed class JWTLoginNameProvider : ILoginNameProvider
{
    IHttpContextAccessor _httpContextAccessor;

    public JWTLoginNameProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type.ToLower() == "name").Select(c => c.Value).FirstOrDefault(); }
}
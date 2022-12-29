using System.Linq;
using Microsoft.AspNetCore.Http;
using Vouchers.Application.Infrastructure;

namespace Vouchers.API.Services;

public sealed class JWTLoginNameProvider : ILoginNameProvider
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public JWTLoginNameProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type.ToLower() == "name").Select(c => c.Value).FirstOrDefault(); }
}
using Microsoft.AspNetCore.Http;
using System.Linq;
using Vouchers.Application.Infrastructure;

namespace Vouchers.API.Services
{
    public sealed class LoginService : ILoginService
    {
        IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Identity.Name; }
    }

    public sealed class JWTLoginService : ILoginService
    {
        IHttpContextAccessor _httpContextAccessor;

        public JWTLoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLoginName { get => _httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type.ToLower() == "name").Select(c => c.Value).FirstOrDefault(); }
    }
}

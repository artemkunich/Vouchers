using Microsoft.AspNetCore.Http;
using System.Linq;
using Vouchers.Application.Infrastructure;

namespace Vouchers.API.Services
{
    public class LoginService : ILoginService
    {
        IHttpContextAccessor httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLoginName { get => httpContextAccessor.HttpContext.User.Identity.Name; }
    }

    public class JWTLoginService : ILoginService
    {
        IHttpContextAccessor httpContextAccessor;

        public JWTLoginService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLoginName { get => httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type.ToLower() == "name").Select(c => c.Value).FirstOrDefault(); }
    }
}

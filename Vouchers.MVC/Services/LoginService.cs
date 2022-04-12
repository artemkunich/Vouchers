using Microsoft.AspNetCore.Http;
using Vouchers.Application.Infrastructure;

namespace Vouchers.MVC.Services
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
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public sealed class IdentityController : Controller
    {
        private readonly IDispatcher _dispatcher;
        private readonly ILoginService _loginService;
        public IdentityController(IDispatcher dispatcher, ILoginService loginService)
        {
            _dispatcher = dispatcher;
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginName = _loginService.CurrentLoginName;
            var identityId = await _dispatcher.DispatchAsync<string, Guid?>(loginName);
            if (identityId is null)
                return NotFound();

            return Json(
                new { 
                    IdentityId = identityId 
                }
            );
        }
    }
}

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
    public class IdentityController : Controller
    {
        private readonly IDispatcher dispatcher;
        private readonly ILoginService loginService;
        public IdentityController(IDispatcher dispatcher, ILoginService loginService)
        {
            this.dispatcher = dispatcher;
            this.loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginName = loginService.CurrentLoginName;
            var identityId = await dispatcher.DispatchAsync<string, Guid?>(loginName);
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

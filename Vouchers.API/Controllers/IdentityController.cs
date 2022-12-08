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
        private readonly ILoginNameProvider _loginNameProvider;
        public IdentityController(IDispatcher dispatcher, ILoginNameProvider loginNameProvider)
        {
            _dispatcher = dispatcher;
            _loginNameProvider = loginNameProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var loginName = _loginNameProvider.CurrentLoginName;
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Authorize]
    public sealed class IdentityDetailController : Controller
    {
        private readonly IDispatcher _dispatcher;
        private readonly ILoginService _loginService;

        public IdentityDetailController(IDispatcher dispatcher, ILoginService loginService)
        {
            _dispatcher = dispatcher;
            _loginService = loginService;
        }

        [HttpGet]
        [Route("[controller]/{accountId:guid?}")]
        public async Task<IActionResult> Get(Guid? accountId)
        {
            try
            {
                var detail = await _dispatcher.DispatchAsync<Guid?, IdentityDetailDto>(accountId);
                if (detail is null)
                    return NotFound();

                return Json(detail);
            }
            catch (NotRegisteredException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> Post([FromForm] IdentityDetailDto identityDetailDto)
        {
            var loginName = _loginService.CurrentLoginName;
            var identityId = await _dispatcher.DispatchAsync<string, Guid?>(loginName);

            if (identityId is null)
            {
                await _dispatcher.DispatchAsync(new CreateIdentityCommand { LoginName = loginName, IdentityDetail = identityDetailDto });
                identityId = await _dispatcher.DispatchAsync<string, Guid?>(loginName);
                return Json(new { identityId });
            }
            else
            {
                await _dispatcher.DispatchAsync(new UpdateIdentityCommand { IdentityDetail = identityDetailDto });
            }

            return NoContent();
        }
    }
}

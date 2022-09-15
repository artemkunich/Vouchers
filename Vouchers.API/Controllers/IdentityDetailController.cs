using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Commands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IdentityDetailController : Controller
    {
        private readonly IDispatcher dispatcher;
        private readonly ILoginService loginService;

        public IdentityDetailController(IDispatcher dispatcher, ILoginService loginService)
        {
            this.dispatcher = dispatcher;
            this.loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? identityId)
        {
            try
            {
                var detail = await dispatcher.DispatchAsync<Guid?, IdentityDetailDto>(identityId);
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
        public async Task<IActionResult> Post([FromForm] IdentityDetailDto identityDetailDto)
        {
            var loginName = loginService.CurrentLoginName;
            var identityId = await dispatcher.DispatchAsync<string, Guid?>(loginName);

            if (identityId is null)
            {
                await dispatcher.DispatchAsync(new CreateIdentityCommand { LoginName = loginName, IdentityDetail = identityDetailDto });
                identityId = await dispatcher.DispatchAsync<string, Guid?>(loginName);
                return Json(new { identityId });
            }
            else
            {
                await dispatcher.DispatchAsync(new UpdateIdentityCommand { IdentityDetail = identityDetailDto });
            }

            return NoContent();
        }
    }
}

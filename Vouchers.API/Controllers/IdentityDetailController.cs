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

        public IdentityDetailController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
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
        public async Task<IActionResult> Post([FromForm] CreateIdentityCommand command)
        {
            
            var identityId = await _dispatcher.DispatchAsync<CreateIdentityCommand, Guid>(command);
            return Json(new { identityId });
        }

        [HttpPut]
        [Route("[controller]")]
        public async Task<IActionResult> Put([FromForm] UpdateIdentityCommand command)
        {
            await _dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

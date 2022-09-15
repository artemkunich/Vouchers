using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public class DomainAccountsController : Controller
    {
        private readonly IDispatcher dispatcher;

        public DomainAccountsController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DomainAccountsQuery query)
        {
            var result = await dispatcher.DispatchAsync<DomainAccountsQuery, IEnumerable<DomainAccountDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDomainAccountCommand command)
        {
            var domainAccountId = await dispatcher.DispatchAsync<CreateDomainAccountCommand, Guid>(command);
            return Json(new { DomainAccountId = domainAccountId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDomainAccountCommand command)
        {
            await dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

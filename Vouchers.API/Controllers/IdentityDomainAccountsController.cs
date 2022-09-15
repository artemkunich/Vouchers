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
    public class IdentityDomainAccountsController : Controller
    {
        private readonly IDispatcher dispatcher;

        public IdentityDomainAccountsController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IdentityDomainAccountsQuery query)
        {
            var result = await dispatcher.DispatchAsync<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>>(query);
            return Json(result);
        }
    }
}

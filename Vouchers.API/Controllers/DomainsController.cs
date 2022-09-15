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
    public class DomainsController : Controller
    {
        private readonly IDispatcher dispatcher;

        public DomainsController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DomainsQuery query)
        {
            var result = await dispatcher.DispatchAsync<DomainsQuery, IEnumerable<DomainDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDomainCommand command)
        {
            var domainId = await dispatcher.DispatchAsync<CreateDomainCommand, Guid?>(command);

            if (domainId is null)
                return Accepted();

            return Json(new { DomainId = domainId });
        }
    }
}

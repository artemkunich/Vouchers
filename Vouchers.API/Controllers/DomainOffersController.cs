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
    [Authorize(Roles = "Manager")]
    public class DomainOffersController : Controller
    {
        private readonly IDispatcher dispatcher;

        public DomainOffersController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DomainOffersQuery query)
        {
            var result = await dispatcher.DispatchAsync<DomainOffersQuery, IEnumerable<DomainOfferDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDomainOfferCommand command)
        {
            var offerId = await dispatcher.DispatchAsync<CreateDomainOfferCommand, Guid>(command);
            return Json(new { OfferId = offerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDomainOfferCommand command)
        {
            await dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

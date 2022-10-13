using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public sealed class DomainOffersController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public DomainOffersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DomainOffersQuery query)
        {
            var result = await _dispatcher.DispatchAsync<DomainOffersQuery, IEnumerable<DomainOfferDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDomainOfferCommand command)
        {
            var offerId = await _dispatcher.DispatchAsync<CreateDomainOfferCommand, Guid>(command);
            return Json(new { OfferId = offerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDomainOfferCommand command)
        {
            await _dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

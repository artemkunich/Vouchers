using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public sealed class IssuerValuesController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public IssuerValuesController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query)
        {
            var result = await _dispatcher.DispatchAsync<IssuerValuesQuery, IEnumerable<VoucherValueDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]CreateVoucherValueCommand command)
        {
            var voucherValueId = await _dispatcher.DispatchAsync<CreateVoucherValueCommand, Guid>(command);
            return Json(new { VoucherValueId = voucherValueId });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm]UpdateVoucherValueCommand command)
        {
            await _dispatcher.DispatchAsync(command);
            return Accepted();
        }
    }
}

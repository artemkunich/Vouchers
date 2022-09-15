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
    public class IssuerValuesController : Controller
    {
        private readonly IDispatcher dispatcher;

        public IssuerValuesController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query)
        {
            var result = await dispatcher.DispatchAsync<IssuerValuesQuery, IEnumerable<VoucherValueDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]CreateVoucherValueCommand command)
        {
            var voucherValueId = await dispatcher.DispatchAsync<CreateVoucherValueCommand, Guid>(command);
            return Json(new { VoucherValueId = voucherValueId });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm]UpdateVoucherValueCommand command)
        {
            await dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

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
    public class IssuerVouchersController : Controller
    {
        private readonly IDispatcher dispatcher;

        public IssuerVouchersController(IDispatcher dispatcher) =>
            this.dispatcher = dispatcher;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IssuerVouchersQuery query)
        {
            var result = await dispatcher.DispatchAsync<IssuerVouchersQuery, IEnumerable<VoucherDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateVoucherCommand command)
        {
            var voucherId = await dispatcher.DispatchAsync<CreateVoucherCommand, Guid>(command);
            return Json(new { VoucherId = voucherId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateVoucherCommand command)
        {
            await dispatcher.DispatchAsync(command);
            return NoContent();
        }
    }
}

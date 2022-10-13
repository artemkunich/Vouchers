using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public sealed class IssuerVouchersController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public IssuerVouchersController(IDispatcher dispatcher) =>
            _dispatcher = dispatcher;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IssuerVouchersQuery query)
        {
            var result = await _dispatcher.DispatchAsync<IssuerVouchersQuery, IEnumerable<VoucherDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateVoucherCommand command)
        {
            var voucherId = await _dispatcher.DispatchAsync<CreateVoucherCommand, Guid>(command);
            return Json(new { VoucherId = voucherId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateVoucherCommand command)
        {
            await _dispatcher.DispatchAsync(command);
            return Accepted();
        }
    }
}

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
    public class HolderTransactionsController : Controller
    {
        private readonly IDispatcher dispatcher;

        public HolderTransactionsController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] HolderValuesQuery query)
        {
            var result = await dispatcher.DispatchAsync<HolderValuesQuery, IEnumerable<HolderTransactionDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateHolderTransactionCommand command)
        {
            var holderTransactionId = await dispatcher.DispatchAsync<CreateHolderTransactionCommand, Guid>(command);
            return Json(new { HolderTransactionId = holderTransactionId });
        }
    }
}

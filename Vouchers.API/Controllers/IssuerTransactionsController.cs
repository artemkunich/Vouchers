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
    public class IssuerTransactionsController : Controller
    {
        private readonly IDispatcher dispatcher;

        public IssuerTransactionsController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query)
        {
            var result = await dispatcher.DispatchAsync<IssuerValuesQuery, IEnumerable<IssuerTransactionDto>>(query);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateIssuerTransactionCommand command)
        {
            var issuerTransactionId = await dispatcher.DispatchAsync<CreateIssuerTransactionCommand, Guid>(command);
            return Json(new { IssuerTransactionId = issuerTransactionId });
        }
    }
}

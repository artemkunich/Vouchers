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
    public class HolderValuesController : Controller
    {
        private readonly IDispatcher dispatcher;

        public HolderValuesController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] HolderValuesQuery query)
        {
            var result = await dispatcher.DispatchAsync<HolderValuesQuery, IEnumerable<VoucherValueDto>>(query);
            return Json(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public sealed class DomainValuesController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public DomainValuesController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DomainValuesQuery query)
        {
            var result = await _dispatcher.DispatchAsync<DomainValuesQuery, IEnumerable<VoucherValueDto>>(query);
            return Json(result);
        }
    }
}

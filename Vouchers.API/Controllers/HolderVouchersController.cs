using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class HolderVouchersController : Controller
{
    private readonly IDispatcher _dispatcher;

    public HolderVouchersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] HolderVouchersQuery query)
    {
        var result = await _dispatcher.DispatchAsync<HolderVouchersQuery, IEnumerable<VoucherDto>>(query);
        return Json(result);
    }
}
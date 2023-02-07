using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

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
    public async Task<IActionResult> Get([FromQuery] DomainValuesQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<DomainValuesQuery, Result<IEnumerable<VoucherValueDto>>>(query));
}
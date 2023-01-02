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
public sealed class IdentityDomainAccountsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IdentityDomainAccountsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] IdentityDomainAccountsQuery query)
    {
        var result = await _dispatcher.DispatchAsync<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>>(query);
        return Json(result);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class DomainAccountsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public DomainAccountsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DomainAccountsQuery query)
    {
        var result = await _dispatcher.DispatchAsync<DomainAccountsQuery, IEnumerable<DomainAccountDto>>(query);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateDomainAccountCommand command)
    {
        var domainAccountId = await _dispatcher.DispatchAsync<CreateDomainAccountCommand, Guid>(command);
        return Json(new { DomainAccountId = domainAccountId });
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateDomainAccountCommand command)
    {
        await _dispatcher.DispatchAsync(command);
        return NoContent();
    }
}

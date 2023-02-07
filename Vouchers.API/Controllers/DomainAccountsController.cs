using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
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
    public async Task<IActionResult> Get([FromQuery] DomainAccountsQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<DomainAccountsQuery, Result<IEnumerable<DomainAccountDto>>>(query));

    [HttpPost]
    public async Task<IActionResult> Post(CreateDomainAccountCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateDomainAccountCommand, Result<IdDto<Guid>>>(command));

    [HttpPut]
    public async Task<IActionResult> Put(UpdateDomainAccountCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateDomainAccountCommand, Result>(command));
}

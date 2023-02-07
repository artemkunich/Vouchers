using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class DomainsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public DomainsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DomainsQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<DomainsQuery, Result<IEnumerable<DomainDto>>>(query));

    [HttpPost]
    public async Task<IActionResult> Post(CreateDomainCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateDomainCommand, Result<IdDto<Guid>>>(command));
}
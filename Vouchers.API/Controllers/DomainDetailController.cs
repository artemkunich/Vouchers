using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
//[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class DomainDetailController : Controller
{
    private readonly IDispatcher _dispatcher;

    public DomainDetailController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Route("[controller]/{domainId:guid}")]
    public async Task<IActionResult> Get(Guid domainId) =>
        this.FromResult(await _dispatcher.DispatchAsync<Guid, Result<DomainDetailDto>>(domainId));

    [HttpPut]
    [Route("[controller]")]
    public async Task<IActionResult> Put([FromForm] UpdateDomainDetailCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateDomainDetailCommand, Result>(command));
}

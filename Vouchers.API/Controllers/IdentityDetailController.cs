using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Authorize]
public sealed class IdentityDetailController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IdentityDetailController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Route("[controller]/{accountId:guid?}")]
    public async Task<IActionResult> Get(Guid? accountId) =>
        this.FromResult(await _dispatcher.DispatchAsync<Guid?, Result<IdentityDetailDto>>(accountId));

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> Post([FromForm] CreateIdentityCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateIdentityCommand, Result<IdDto<Guid>>>(command));
    

    [HttpPut]
    [Route("[controller]")]
    public async Task<IActionResult> Put([FromForm] UpdateIdentityCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateIdentityCommand, Result>(command));

}
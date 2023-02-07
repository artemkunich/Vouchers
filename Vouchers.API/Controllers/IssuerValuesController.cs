using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class IssuerValuesController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IssuerValuesController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<IssuerValuesQuery, Result<IEnumerable<VoucherValueDto>>>(query));

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]CreateVoucherValueCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateVoucherValueCommand, Result<IdDto<Guid>>>(command));

    [HttpPut]
    public async Task<IActionResult> Put([FromForm]UpdateVoucherValueCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateVoucherValueCommand,Result>(command));

}
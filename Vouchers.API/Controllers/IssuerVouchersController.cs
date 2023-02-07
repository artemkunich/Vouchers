using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class IssuerVouchersController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IssuerVouchersController(IDispatcher dispatcher) =>
        _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] IssuerVouchersQuery query) => 
        this.FromResult(await _dispatcher.DispatchAsync<IssuerVouchersQuery, Result<IEnumerable<VoucherDto>>>(query));

    [HttpPost]
    public async Task<IActionResult> Post(CreateVoucherCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateVoucherCommand, Result<IdDto<Guid>>>(command));

    [HttpPut]
    public async Task<IActionResult> Put(UpdateVoucherCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateVoucherCommand, Result>(command));

}
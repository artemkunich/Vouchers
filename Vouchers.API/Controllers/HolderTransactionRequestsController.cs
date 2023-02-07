using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class HolderTransactionRequestsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public HolderTransactionRequestsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] HolderTransactionRequestsQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<HolderTransactionRequestsQuery, Result<IEnumerable<HolderTransactionRequestDto>>>(query));


    [HttpPost]
    public async Task<IActionResult> Post(CreateHolderTransactionRequestCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateHolderTransactionRequestCommand, Result<IdDto<Guid>>>(command));


    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteHolderTransactionRequestCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<DeleteHolderTransactionRequestCommand,Result>(command));
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.IssuerTransactionCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class IssuerTransactionsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IssuerTransactionsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<IssuerValuesQuery, Result<IEnumerable<IssuerTransactionDto>>>(query));

    [HttpPost]
    public async Task<IActionResult> Post(CreateIssuerTransactionCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateIssuerTransactionCommand, Result<IdDto<Guid>>>(command));
}
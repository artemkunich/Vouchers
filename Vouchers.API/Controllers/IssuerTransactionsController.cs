using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async Task<IActionResult> Get([FromQuery] IssuerValuesQuery query)
    {
        var result = await _dispatcher.DispatchAsync<IssuerValuesQuery, IEnumerable<IssuerTransactionDto>>(query);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateIssuerTransactionCommand command)
    {
        var issuerTransactionId = await _dispatcher.DispatchAsync<CreateIssuerTransactionCommand, Guid>(command);
        return Json(new { IssuerTransactionId = issuerTransactionId });
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.HolderTransactionCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class HolderTransactionsController : Controller
{
    private readonly IDispatcher _dispatcher;

    public HolderTransactionsController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] HolderTransactionsQuery query)
    {
        var result = await _dispatcher.DispatchAsync<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>>(query);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateHolderTransactionCommand command)
    {
        var holderTransactionId = await _dispatcher.DispatchAsync<CreateHolderTransactionCommand, Guid>(command);
        return Json(new { HolderTransactionId = holderTransactionId });
    }
}
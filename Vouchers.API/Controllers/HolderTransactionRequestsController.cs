using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async Task<IActionResult> Get([FromQuery] HolderTransactionRequestsQuery query)
    {
        var result = await _dispatcher.DispatchAsync<HolderTransactionRequestsQuery, IEnumerable<HolderTransactionRequestDto>>(query);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateHolderTransactionRequestCommand command)
    {
        var holderTransactionRequestId = await _dispatcher.DispatchAsync<CreateHolderTransactionRequestCommand, Guid>(command);
        return Json(new { HolderTransactionRequestId = holderTransactionRequestId });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteHolderTransactionRequestCommand command)
    {
        await _dispatcher.DispatchAsync(command);
        return NoContent();
    }
}
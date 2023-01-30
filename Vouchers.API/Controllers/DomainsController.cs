using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;
using Vouchers.Primitives;

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
    public async Task<IActionResult> Get([FromQuery] DomainsQuery query)
    {
        var result = await _dispatcher.DispatchAsync<DomainsQuery, IEnumerable<DomainDto>>(query);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateDomainCommand command)
    {
        var createDomainResult = await _dispatcher.DispatchAsync<CreateDomainCommand, Result<Guid?>>(command);

        if (createDomainResult.IsFailure)
            return BadRequest(new {createDomainResult.Errors});

        var domainId = createDomainResult.Value;
        if (domainId is null)
            return Accepted();

        return Json(new { DomainId = domainId });
    }
}
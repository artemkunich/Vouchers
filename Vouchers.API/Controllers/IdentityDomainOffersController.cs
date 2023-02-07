using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "User")]
public sealed class IdentityDomainOffersController : Controller
{
    private readonly IDispatcher _dispatcher;

    public IdentityDomainOffersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] IdentityDomainOffersQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<IdentityDomainOffersQuery, Result<IEnumerable<DomainOfferDto>>>(query));
}
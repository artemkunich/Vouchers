using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.API.Services;
using Vouchers.Application;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Infrastructure;

namespace Vouchers.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Manager")]
public sealed class DomainOffersController : Controller
{
    private readonly IDispatcher _dispatcher;
     public DomainOffersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
     
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DomainOffersQuery query) =>
        this.FromResult(await _dispatcher.DispatchAsync<DomainOffersQuery, Result<IEnumerable<DomainOfferDto>>>(query));
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateDomainOfferCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<CreateDomainOfferCommand, Result<IdDto<Guid>>>(command));

    [HttpPut]
    public async Task<IActionResult> Put(UpdateDomainOfferCommand command) =>
        this.FromResult(await _dispatcher.DispatchAsync<UpdateDomainOfferCommand, Result>(command));
}
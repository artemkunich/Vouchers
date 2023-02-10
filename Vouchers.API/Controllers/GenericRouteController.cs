using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericRouteController<TQuery, TDto> : Controller where TDto : class 
{
    private readonly IHandler<TQuery,TDto> _handler;

    public GenericRouteController(IHandler<TQuery,TDto> handler)
    {
        _handler = handler;
    }
    
    public async Task<IActionResult> HandleQuery([FromRoute]TQuery query, CancellationToken cancellationToken) => 
        this.FromResult(await _handler.HandleAsync(query,cancellationToken));
}
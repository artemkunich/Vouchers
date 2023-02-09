using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers;

public class GenericQueryController<TQuery, TDtos> : Controller where TQuery : ListQuery where TDtos : class 
{
    private readonly IHandler<TQuery,TDtos> _handler;

    public GenericQueryController(IHandler<TQuery,TDtos> handler)
    {
        _handler = handler;
    }
    
    public async Task<IActionResult> HandleQuery([FromQuery]TQuery query, CancellationToken cancellationToken) => 
        this.FromResult(await _handler.HandleAsync(query,cancellationToken));
}
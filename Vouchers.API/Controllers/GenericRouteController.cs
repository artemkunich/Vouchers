using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Infrastructure.Pipeline;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericRouteController<TQuery, TDto> : Controller where TDto : class 
{
    private readonly IPipeline<TQuery,TDto> _pipeline;

    public GenericRouteController(IPipeline<TQuery,TDto> pipeline)
    {
        _pipeline = pipeline;
    }
    
    public async Task<IActionResult> HandleQuery([FromRoute]TQuery query, CancellationToken cancellationToken) => 
        this.FromResult(await _pipeline.HandleAsync(query,cancellationToken));
}
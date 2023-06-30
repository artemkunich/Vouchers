using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Domains.Application.Queries;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericQueryController<TQuery, TDtos> : Controller where TQuery : ListQuery where TDtos : class 
{
    private readonly IPipeline<TQuery,TDtos> _pipeline;

    public GenericQueryController(IPipeline<TQuery,TDtos> pipeline)
    {
        _pipeline = pipeline;
    }
    
    public async Task<IActionResult> HandleQuery([FromQuery]TQuery query, CancellationToken cancellationToken) => 
        this.FromResult(await _pipeline.HandleAsync(query,cancellationToken));
}
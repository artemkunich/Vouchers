using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Application.PipelineBehaviors;
using Vouchers.Application.UseCases;
using Vouchers.Infrastructure.Pipeline;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericCommandJsonController<TCommand, TDto> : Controller where TCommand : class where TDto : class 
{
    private readonly IPipeline<TCommand,TDto> _pipeline;

    public GenericCommandJsonController(IPipeline<TCommand,TDto> pipeline)
    {
        _pipeline = pipeline;
    }
    
    public async Task<IActionResult> HandleCommand([FromBody]TCommand command, CancellationToken cancellationToken) => 
        this.FromResult(await _pipeline.HandleAsync(command,cancellationToken));
}
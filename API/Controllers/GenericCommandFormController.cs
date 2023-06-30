using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericCommandFormController<TCommand, TDto> : Controller where TCommand : class where TDto : class 
{
    private readonly IPipeline<TCommand,TDto> _pipeline;

    public GenericCommandFormController(IPipeline<TCommand,TDto> pipeline)
    {
        _pipeline = pipeline;
    }
    
    public async Task<IActionResult> HandleCommand([FromForm]TCommand command, CancellationToken cancellationToken) => 
        this.FromResult(await _pipeline.HandleAsync(command,cancellationToken));
}
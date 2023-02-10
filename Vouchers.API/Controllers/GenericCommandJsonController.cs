using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vouchers.API.Services;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Controllers;

[Authorize]
public class GenericCommandJsonController<TCommand, TDto> : Controller where TCommand : class where TDto : class 
{
    private readonly IHandler<TCommand,TDto> _handler;

    public GenericCommandJsonController(IHandler<TCommand,TDto> handler)
    {
        _handler = handler;
    }
    
    public async Task<IActionResult> HandleCommand([FromBody]TCommand command, CancellationToken cancellationToken) => 
        this.FromResult(await _handler.HandleAsync(command,cancellationToken));
}

[Authorize]
public class GenericCommandJsonController<TCommand> : Controller where TCommand : class
{
    private readonly IHandler<TCommand> _handler;

    public GenericCommandJsonController(IHandler<TCommand> handler)
    {
        _handler = handler;
    }
    
    public async Task<IActionResult> HandleCommand([FromBody]TCommand command, CancellationToken cancellationToken) => 
        this.FromResult(await _handler.HandleAsync(command, cancellationToken));
}
using Vouchers.Application.Abstractions;

namespace Vouchers.Infrastructure.Pipeline;

public sealed class GenericPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
{
    private readonly Func<TRequest, CancellationToken, Task<Result<TResponse>>> _next;
    
    public GenericPipeline(IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors, IHandler<TRequest, TResponse> handler)
    {
        var reversedBehaviors = behaviors.Reverse();

        _next = handler.HandleAsync;
        foreach (var behavior in reversedBehaviors)
        {
            var next = _next;
            _next = (req, token) => behavior.HandleAsync(req, token, async () => await next(req, token));
        }
    }
    
    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken token) => 
        await _next(request, token);
}
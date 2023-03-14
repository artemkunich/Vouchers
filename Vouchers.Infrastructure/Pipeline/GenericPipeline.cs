using Vouchers.Application.Abstractions;

namespace Vouchers.Infrastructure.Pipeline;

public sealed class GenericPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Func<TRequest, CancellationToken, Task<Result<TResponse>>> _next;
    
    public GenericPipeline(IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors, IRequestHandler<TRequest, TResponse> requestHandler)
    {
        var reversedBehaviors = behaviors.Reverse();

        _next = requestHandler.HandleAsync;
        foreach (var behavior in reversedBehaviors)
        {
            var next = _next;
            _next = (req, token) => behavior.HandleAsync(req, token, async () => await next(req, token));
        }
    }
    
    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation) => 
        await _next(request, cancellation);
}
using Akunich.Application.Abstractions;
using Vouchers.Persistence.PipelineBehaviors;

namespace Vouchers.Infrastructure.Pipelines;

public class DefaultPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private IPipeline<TRequest, TResponse> _pipeline;
    
    public DefaultPipeline(
        DbUpdateConcurrencyExceptionBehavior<TRequest, TResponse> dbUpdateConcurrencyExceptionBehavior,
        DbSaveChangesBehavior<TRequest, TResponse> dbSaveBehavior,
        IRequestHandler<TRequest,TResponse> handler
        )
    {
        _pipeline = PipelineBuilder<TRequest, TResponse>.Create()
            .AddBehavior(dbUpdateConcurrencyExceptionBehavior)
            .AddBehavior(dbSaveBehavior)
            .SetHandler(handler)
            .Build();
    }

    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
    
}
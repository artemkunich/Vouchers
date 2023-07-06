using Akunich.Application.Abstractions;
using Vouchers.Persistence.PipelineBehaviors;

namespace Vouchers.Root.Pipelines;

public class DefaultPipeline<TRequest, TResponse> : Pipeline<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public DefaultPipeline(
        IRequestHandler<TRequest,TResponse> handler,
        DbUpdateConcurrencyExceptionBehavior<TRequest, TResponse> dbUpdateConcurrencyExceptionBehavior,
        DbSaveChangesBehavior<TRequest, TResponse> dbSaveBehavior
    ) : base(handler, dbUpdateConcurrencyExceptionBehavior, dbSaveBehavior)
    {
    }
}
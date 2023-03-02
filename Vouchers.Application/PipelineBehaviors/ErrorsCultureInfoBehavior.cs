using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Application.PipelineBehaviors;

public class ErrorsCultureInfo<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
{
    private readonly IResourceProvider _resourceProvider;

    public ErrorsCultureInfo(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> next)
    {
        var result = await next();

        if (result.IsSuccess)
            return result;

        foreach (var error in result.Errors)
        {
            error.Message = _resourceProvider.GetString(error.Code);
        }

        return result;
    }
}
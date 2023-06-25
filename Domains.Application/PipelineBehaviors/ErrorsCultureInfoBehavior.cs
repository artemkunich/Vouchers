using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Extensions.Resources;

namespace Vouchers.Domains.Application.PipelineBehaviors;

public class ErrorsCultureInfo<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IResourceProvider _resourceProvider;

    public ErrorsCultureInfo(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, ShortNextDelegate<TResponse> nextAsync)
    {
        var result = await nextAsync();

        if (result.IsSuccess)
            return result;

        foreach (var error in result.Errors)
        {
            error.Message = _resourceProvider.GetString(error.Code);
        }

        return result;
    }
}
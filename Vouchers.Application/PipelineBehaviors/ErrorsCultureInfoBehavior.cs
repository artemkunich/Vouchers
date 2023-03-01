using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Errors;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;

namespace Vouchers.Application.PipelineBehaviors;

[PipelineBehaviorPriority(9)]
public class ErrorsCultureInfo<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
{
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public ErrorsCultureInfo(ICultureInfoProvider cultureInfoProvider)
    {
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> next)
    {
        var result = await next();

        if (result.IsSuccess)
            return result;

        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        if (cultureInfo is null)
            return result;
        
        foreach (var error in result.Errors)
        {
            error.Message = ApplicationResources.GetString(error.Code, cultureInfo);
        }

        return result;
    }
}
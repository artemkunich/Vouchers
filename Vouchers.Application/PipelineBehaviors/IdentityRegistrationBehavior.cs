using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;

namespace Vouchers.Application.PipelineBehaviors;

[PipelineBehaviorPriority(10)]
public class IdentityRegistrationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public IdentityRegistrationBehavior(IAuthIdentityProvider authIdentityProvider, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> next)
    {
        if (typeof(TRequest) == typeof(CreateIdentityCommand))
        {
            return await next();
        }

        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId == Guid.Empty)
            return Error.NotRegistered(_cultureInfoProvider.GetCultureInfo());
        
        return await next();
    }
}
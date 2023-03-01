using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Errors;
using Vouchers.Application.Services;

namespace Vouchers.Application.PipelineBehaviors;

public class IdentityRegistrationBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;

    public IdentityRegistrationBehavior(IAuthIdentityProvider authIdentityProvider)
    {
        _authIdentityProvider = authIdentityProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> next)
    {
        if (typeof(TRequest) == typeof(CreateIdentityCommand))
        {
            return await next();
        }

        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId == Guid.Empty)
            return new NotRegisteredError();
        
        return await next();
    }
}
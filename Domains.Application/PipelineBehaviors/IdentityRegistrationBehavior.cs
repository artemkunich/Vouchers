using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Application.UseCases.IdentityCases;

namespace Vouchers.Domains.Application.PipelineBehaviors;

public class IdentityRegistrationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;

    public IdentityRegistrationBehavior(IIdentityIdProvider<Guid> identityIdProvider)
    {
        _identityIdProvider = identityIdProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, ShortNextDelegate<TResponse> nextAsync)
    {
        if (typeof(TRequest) == typeof(CreateIdentityCommand))
        {
            return await nextAsync();
        }

        var authIdentityId = _identityIdProvider.GetIdentityId();
        if (authIdentityId == Guid.Empty)
            return new NotRegisteredError();
        
        return await nextAsync();
    }
}
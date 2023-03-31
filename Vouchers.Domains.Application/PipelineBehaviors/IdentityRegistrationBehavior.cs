using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Application.UseCases.IdentityCases;

namespace Vouchers.Domains.Application.PipelineBehaviors;

public class IdentityRegistrationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IIdentityIdProvider _identityIdProvider;

    public IdentityRegistrationBehavior(IIdentityIdProvider identityIdProvider)
    {
        _identityIdProvider = identityIdProvider;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> nextAsync)
    {
        if (typeof(TRequest) == typeof(CreateIdentityCommand))
        {
            return await nextAsync();
        }

        var authIdentityId = _identityIdProvider.CurrentIdentityId;
        if (authIdentityId == Guid.Empty)
            return new NotRegisteredError();
        
        return await nextAsync();
    }
}
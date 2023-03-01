using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Identities.Domain.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedEventHandler : IEventHandler<IdentityUpdatedEvent>
{
    public IdentityUpdatedEventHandler()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedEvent @event, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}

public class IdentityUpdatedEventHandler2 : IEventHandler<IdentityUpdatedEvent>
{
    public IdentityUpdatedEventHandler2()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedEvent @event, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}
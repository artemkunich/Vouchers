using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedDomainEventHandler : IDomainEventHandler<IdentityUpdatedDomainEvent>
{
    public IdentityUpdatedDomainEventHandler()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedDomainEvent domainEvent, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}

public class IdentityUpdatedDomainEventHandler2 : IDomainEventHandler<IdentityUpdatedDomainEvent>
{
    public IdentityUpdatedDomainEventHandler2()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedDomainEvent domainEvent, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}

public class IdentityUpdatedIntegrationEventHandler : IIntegrationEventHandler<IdentityUpdatedIntegrationEvent>
{
    public IdentityUpdatedIntegrationEventHandler()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedIntegrationEvent @event, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}
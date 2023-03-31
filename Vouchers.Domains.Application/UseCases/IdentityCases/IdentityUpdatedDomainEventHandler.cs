using System.Threading;
using System.Threading.Tasks;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.DomainEvents;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

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
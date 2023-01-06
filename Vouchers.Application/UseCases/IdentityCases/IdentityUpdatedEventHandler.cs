using System.Threading;
using System.Threading.Tasks;
using Vouchers.Identities.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedEventHandler : IHandler<IdentityUpdatedDomainEvent>
{
    public IdentityUpdatedEventHandler()
    {
    }
    
    public async Task HandleAsync(IdentityUpdatedDomainEvent @event, CancellationToken cancellation)
    {
    }
}
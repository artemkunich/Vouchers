using System.Threading;
using System.Threading.Tasks;
using Vouchers.Identities.Domain.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedEventHandler : IHandler<IdentityUpdatedDomainEvent>
{
    public IdentityUpdatedEventHandler()
    {
    }
    
    public async Task<Result> HandleAsync(IdentityUpdatedDomainEvent @event, CancellationToken cancellation)
    {
        return Result.Create();
    }
}
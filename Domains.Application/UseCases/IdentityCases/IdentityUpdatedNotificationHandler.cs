using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.DomainEvents;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

public class IdentityUpdatedNotificationHandler : INotificationHandler<IdentityUpdatedNotification>
{
    public IdentityUpdatedNotificationHandler()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedNotification notification, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}

public class IdentityUpdatedNotificationHandler2 : INotificationHandler<IdentityUpdatedNotification>
{
    public IdentityUpdatedNotificationHandler2()
    {
    }
    
    public async Task<Result<Unit>> HandleAsync(IdentityUpdatedNotification notification, CancellationToken cancellation)
    {
        return Unit.Value;
    }
}
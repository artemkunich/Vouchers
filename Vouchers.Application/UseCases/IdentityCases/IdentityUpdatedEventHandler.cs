using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Events.IdentityEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

public class IdentityUpdatedEventHandler : IHandler<IdentityUpdatedEvent>
{
    public Task HandleAsync(IdentityUpdatedEvent request, CancellationToken cancellation)
    {
        Console.WriteLine("Hello from IdentityUpdatedEventHandler");
        
        return Task.CompletedTask;
    }
}
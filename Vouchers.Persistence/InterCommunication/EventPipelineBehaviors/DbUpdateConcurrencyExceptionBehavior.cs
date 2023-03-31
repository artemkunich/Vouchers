using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Persistence.InterCommunication.Errors;

namespace Vouchers.Persistence.InterCommunication.EventPipelineBehaviors;

public class DbUpdateConcurrencyExceptionBehavior<TEvent> : IIntegrationEventPipelineBehavior<TEvent>
{
    public async Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync)
    {
        var remainingAttempts = 3;

        while (remainingAttempts > 0)
        {
            try
            {
                return await nextAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                remainingAttempts--;
            }
        }

        return new DbUpdateConcurrencyError();
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Abstractions;
using Vouchers.Persistence.InterCommunication.Errors;

namespace Vouchers.Persistence.InterCommunication.EventPipelineBehaviors;

public class DbUpdateConcurrencyExceptionBehavior<TEvent> : IEventPipelineBehavior<TEvent>
{
    public async Task<Result<Unit>> HandleAsync(TEvent request, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync)
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
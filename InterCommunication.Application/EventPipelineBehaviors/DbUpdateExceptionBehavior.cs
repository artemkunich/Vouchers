using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Persistence.InterCommunication.Errors;

namespace Vouchers.Persistence.InterCommunication.EventPipelineBehaviors;

public class DbUpdateExceptionBehavior<TEvent> : IIntegrationEventPipelineBehavior<TEvent>
{
    public async Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation, HandlerDelegate<Unit> nextAsync)
    {
        try
        {
            return await nextAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.GetBaseException() is not SqlException {Number: 2627 or 2601})
            {
                return Unit.Value;
            }

            return new DbUpdateError();
        }
    }
}
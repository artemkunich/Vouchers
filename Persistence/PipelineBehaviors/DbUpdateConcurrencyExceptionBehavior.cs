using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Persistence.InterCommunication.Errors;

namespace Vouchers.Persistence.PipelineBehaviors;

public class DbUpdateConcurrencyExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly VouchersDbContext _dbContext;
    
    public DbUpdateConcurrencyExceptionBehavior(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> nextAsync)
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
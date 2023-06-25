using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;

namespace Vouchers.Persistence.PipelineBehaviors;

public class DbSaveChangesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly VouchersDbContext _dbContext;
    
    public DbSaveChangesBehavior(VouchersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<TResponse> nextAsync)
    {
        var result = await nextAsync();
        if(result.IsSuccess && _dbContext.ChangeTracker.HasChanges())
            await _dbContext.SaveChangesAsync(cancellation);

        return result;
    }
}
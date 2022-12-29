using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Application.Infrastructure;
using Vouchers.InterCommunication;
using Vouchers.MinimalAPI.EventRouters;

namespace Vouchers.MinimalAPI.Services;

public class OutboxMessagesProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxMessagesProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var lastProcessedMessagesCount = await ProcessOutboxMessagesAsync(stoppingToken);
            
            if(lastProcessedMessagesCount == 0)
                await Task.Delay(TimeSpan.FromMilliseconds(3000), stoppingToken);
        }
    }

    private async Task<int> ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var outboxMessageRepository = scope.ServiceProvider.GetService<IRepository<OutboxMessage,Guid>>();
        var outboxMessages = await outboxMessageRepository.GetByExpressionAsync(e => e.State == OutboxMessageState.Ready);

        var processedMessagesCount = 0;
        
        if (outboxMessages.IsNullOrEmpty())
            return processedMessagesCount;
        
        
        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                var eventRouter = scope.ServiceProvider.GetEventRoute(outboxMessage.Type);
                await eventRouter.RouteAsync(outboxMessage.Data, stoppingToken);
                outboxMessage.Process();
                await outboxMessageRepository.UpdateAsync(outboxMessage);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                break;
            }
            catch (DbUpdateException ex)
            {
                if (ex.GetBaseException() is not SqlException {Number: 2627 or 2601})
                {
                    outboxMessage.Process();
                }
                else
                {
                    throw;
                }
            }

            processedMessagesCount++;
        }

        return processedMessagesCount;
    }
}
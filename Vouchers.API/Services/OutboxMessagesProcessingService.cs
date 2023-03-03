using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.API.Services;

public class OutboxMessagesProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public OutboxMessagesProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            var lastProcessedMessagesCount = await ProcessOutboxMessagesAsync(cancellation);
            
            if(lastProcessedMessagesCount == 0)
                await Task.Delay(TimeSpan.FromMilliseconds(3000), cancellation);
        }
    }

    private async Task<int> ProcessOutboxMessagesAsync(CancellationToken cancellation)
    {
        using var scope = _serviceProvider.CreateScope();
        var messagesProcessor = scope.ServiceProvider.GetService<IMessagesProcessor>();
        if (messagesProcessor is null)
            return 0;

        return await messagesProcessor.ProcessMessagesAsync(cancellation);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.InterCommunication;
using Vouchers.Infrastructure;
using Vouchers.Persistence;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.API.Services;

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
        var serviceProvider = scope.ServiceProvider;
        var dbContext = scope.ServiceProvider.GetService<VouchersDbContext>();
        if (dbContext is null)
            return 0;
        
        var outboxMessages = await dbContext.Set<OutboxMessage>().Where(x => x.State == OutboxMessageState.Ready).Take(100).ToListAsync(stoppingToken);

        var processedMessagesCount = 0;
        
        if (outboxMessages.IsNullOrEmpty())
            return processedMessagesCount;
        
        
        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Vouchers"));

                Type eventType = null;
                foreach (var assembly in assemblies)
                {
                    eventType = assembly.GetTypes().FirstOrDefault(t => t.Name == outboxMessage.Type);
                    if (eventType is not null)
                        break; 
                }
                
                if (eventType is null)
                    break;
                
                var messageHandlerType = typeof(IMessageHandler<>).MakeGenericType(eventType);

                var messageHandler = serviceProvider.GetService(messageHandlerType);
                if (messageHandler is null)
                    break;

                var @event = JsonSerializer.Deserialize(outboxMessage.Data,eventType,JsonSerializerOptions.Default);
                if (@event is null)
                    break;

                var handleMethod = messageHandlerType.GetMethod(nameof(IMessageHandler<object>.HandleAsync));
                if(handleMethod is null)
                    break;
                
                if(handleMethod.Invoke(messageHandler, new [] {@event, default(CancellationToken)}) is Task task)
                    await task;

                outboxMessage.MarkAsProcessed();
                dbContext.Set<OutboxMessage>().Update(outboxMessage);
                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                break;
            }
            catch (DbUpdateException ex)
            {
                if (ex.GetBaseException() is not SqlException {Number: 2627 or 2601})
                {
                    outboxMessage.MarkAsProcessed();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Application.Abstractions;
using Vouchers.InterCommunication;
using Vouchers.Primitives;

namespace Vouchers.Persistence.InterCommunication;

public class MessagesProcessor : IMessagesProcessor
{
    private readonly VouchersDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private static readonly Dictionary<string,Type> DomainEventTypes = new ();

    public MessagesProcessor(VouchersDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public static void AddEventTypesFromAssembly(Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(type => !type.IsInterface && !type.IsAbstract && typeof(IEvent).IsAssignableFrom(type)).ToList();
            
        types.ForEach(type =>
        {
            if (type.FullName != null) DomainEventTypes[type.FullName] = type;
        });
    }
    
    public async Task<int> ProcessMessagesAsync(CancellationToken cancellation)
    {
        var outboxMessages = await _dbContext.Set<OutboxMessage>().Where(x => x.State == OutboxMessageState.Ready).Take(100).ToListAsync(cancellation);

        var processedMessagesCount = 0;
        
        if (outboxMessages.IsNullOrEmpty())
            return processedMessagesCount;
        
        
        foreach (var outboxMessage in outboxMessages)
        {
            if (!DomainEventTypes.ContainsKey(outboxMessage.Type))
                break;
            
            var eventType = DomainEventTypes[outboxMessage.Type];

            var messagePipelineType = typeof(IMessagePipeline<>).MakeGenericType(eventType);

            var messagePipeline = _serviceProvider.GetService(messagePipelineType);
            if (messagePipeline is null)
                break;

            var @event = JsonSerializer.Deserialize(outboxMessage.Data, eventType, JsonSerializerOptions.Default);
            if (@event is null)
                break;

            var handleMethod = messagePipelineType.GetMethod(nameof(IMessagePipeline<object>.HandleAsync));
            if(handleMethod is null)
                break;
            
            if (handleMethod.Invoke(messagePipeline, new[] {@event, default(CancellationToken)}) is Task<Result<Unit>> task)
            {
                var result = await task;
                if (result.IsFailure)
                    break;
            }

            outboxMessage.MarkAsProcessed();
            _dbContext.Set<OutboxMessage>().Update(outboxMessage);
            await _dbContext.SaveChangesAsync(cancellation);
            
            processedMessagesCount++;
        }

        return processedMessagesCount;
    }
}
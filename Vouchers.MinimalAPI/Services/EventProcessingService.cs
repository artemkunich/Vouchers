using System.Text.Json;
using Microsoft.AspNetCore.Components.Routing;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Application.UseCases.LoginCommands;
using Vouchers.Entities;
using Vouchers.MinimalAPI.EventRouters;

namespace Vouchers.MinimalAPI.Services;

public class EventProcessingService : BackgroundService
{
    private IServiceProvider _serviceProvider;

    public EventProcessingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(3000), stoppingToken);

            await ProcessOutboxEventsAsync(stoppingToken);
        }
    }

    private async Task ProcessOutboxEventsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var outboxEventRepository = scope.ServiceProvider.GetService<IRepository<OutboxEvent,Guid>>();
        var outboxEvents = outboxEventRepository.GetByExpression(e => e.State == OutboxEventState.Ready);
            
        if (!outboxEvents.Any())
            return;

            
        foreach (var outboxEvent in outboxEvents)
        {
            var eventRouter = scope.ServiceProvider.GetEventRoute(outboxEvent.Type);
            await eventRouter.RouteAsync(outboxEvent.Data, stoppingToken);
            outboxEvent.Process();
            await outboxEventRepository.UpdateAsync(outboxEvent);
        }
    }
}
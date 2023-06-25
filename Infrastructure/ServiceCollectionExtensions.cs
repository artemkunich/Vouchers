using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Time;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Core.Infrastructure;
using Vouchers.Domains.Infrastructure;
using Vouchers.Files.Infrastructure;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Queries.Infrastructure;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddScoped<IMessageHelper, MessageHelper>()
            .AddGuidIdentifierProvider()
            .AddSystemTimeProvider()
            .AddScoped<IRequestDispatcher, RequestDispatcher>()
            .AddScoped<INotificationDispatcher, NotificationDispatcher>();

    public static IServiceCollection AddModules(this IServiceCollection services) => services
        .AddCoreModule()
        .AddDomainsModule()
        .AddFilesModule()
        .AddQueriesModule();

    // public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services, Assembly assembly)
    // {
    //     var handlerTypes = assembly.GetTypes().Where(t =>
    //         !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
    //         t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
    //     ).ToList();
    //
    //     foreach (var handlerType in handlerTypes)
    //     {
    //         var genericHandlerType = handlerType
    //             .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));
    //         if(genericHandlerType is null)
    //             continue;
    //         
    //         services.AddScoped(genericHandlerType, handlerType);
    //
    //         var firstGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[0];
    //
    //         var pipelineInterfaceType = typeof(IIntegrationEventPipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);
    //         var pipelineType = typeof(IntegrationEventPipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);
    //
    //         services.AddScoped(pipelineInterfaceType, pipelineType);
    //     }
    //     
    //     
    //     services.AddScoped<IMessagesProcessor,MessagesProcessor>();
    //     MessagesProcessor.AddEventTypesFromAssembly(assembly);
    //     return services;
    // }
    

 

}

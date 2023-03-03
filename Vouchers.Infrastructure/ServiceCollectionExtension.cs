using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.PipelineBehaviors;
using Vouchers.Application.ServiceProviders;
using Vouchers.Application.Services;
using Vouchers.Infrastructure.EventPipelineBehaviors;
using Vouchers.Infrastructure.Pipeline;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Persistence.InterCommunication.EventPipelineBehaviors;
using Vouchers.Resources;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddScoped<IMessageHelper, MessageHelper>()
            .AddScoped<IIdentifierProvider<Guid>, GuidIdentifierProvider>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IResourceProvider, ResourceProvider>();

    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddScoped<IAppImageService, AppImageService>()
            .AddScoped<IAuthIdentityProvider, AuthIdentityProvider>();

    public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypesTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
        ).ToList();

        foreach (var handlerType in handlerTypesTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);
        }
        
        return services;
    }
    
    public static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypesTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
        ).ToList();

        foreach (var handlerType in handlerTypesTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);

            var firstGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[0];

            var messageHandlerInterfaceType = typeof(IMessagePipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);
            var messageHandlerType = typeof(MessagePipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);

            services.AddScoped(messageHandlerInterfaceType, messageHandlerType);
        }
        services.AddScoped<IMessagesProcessor,MessagesProcessor>();
        return services;
    }
    
    public static IServiceCollection AddRequestPipelineBehaviors(this IServiceCollection services, Assembly assembly) =>
        services
            .AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(ErrorsCultureInfo<,>))
            .AddScoped(typeof(IRequestPipelineBehavior<,>), typeof(IdentityRegistrationBehavior<,>));

    public static IServiceCollection AddEventPipelineBehaviors(this IServiceCollection services, Assembly assembly) =>
        services
            .AddScoped(typeof(IEventPipelineBehavior<>), typeof(DbUpdateExceptionBehavior<>))
            .AddScoped(typeof(IEventPipelineBehavior<>), typeof(DbUpdateConcurrencyExceptionBehavior<>))
            .AddScoped(typeof(IEventPipelineBehavior<>), typeof(TestEventPipelineBehavior<>));
    
    public static IServiceCollection AddGenericPipeline(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IPipeline<,>), typeof(GenericPipeline<,>));
    
}

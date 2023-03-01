using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.PipelineBehaviors;
using Vouchers.Application.ServiceProviders;
using Vouchers.Application.Services;
using Vouchers.Infrastructure.InterCommunication;
using Vouchers.Infrastructure.Pipeline;
using Vouchers.Primitives;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) => 
        services
            .AddScoped<IMessageHelper, MessageHelper>()
            .AddScoped<IIdentifierProvider<Guid>, GuidIdentifierProvider>()
            .AddScoped<IDateTimeProvider, DateTimeProvider>();

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAppImageService, AppImageService>()
            .AddScoped<IAuthIdentityProvider, AuthIdentityProvider>();

        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
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

            var firstGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[0];
            var secondGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[1];
            
            if (typeof(IEvent).IsAssignableFrom(firstGenericHandlerTypeArgument) && secondGenericHandlerTypeArgument == typeof(Unit))
            {
                var messageHandlerInterfaceType = typeof(IMessageHandler<>).MakeGenericType(firstGenericHandlerTypeArgument);
                var messageHandlerType = typeof(MessageHandler<>).MakeGenericType(firstGenericHandlerTypeArgument);

                services.AddScoped(messageHandlerInterfaceType, messageHandlerType);
            }
        }

        return services;
    }
    
    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services, Assembly assembly)
    {
        var pipelineBehaviorGenericTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<,>))
        ).ToList().OrderBy(t => 
            t.CustomAttributes.OfType<PipelineBehaviorPriorityAttribute>().Select(a => a.Priority).FirstOrDefault(uint.MinValue)
        ).ToList();

        foreach (var pipelineBehaviorGenericType in pipelineBehaviorGenericTypes)
        {
            services.AddScoped(typeof(IRequestPipelineBehavior<,>), pipelineBehaviorGenericType);
        }

        return services;
    }
    
    public static IServiceCollection AddEventPipelineBehaviors(this IServiceCollection services, Assembly assembly)
    {
        var pipelineBehaviorGenericTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventPipelineBehavior<>))
        ).ToList().OrderBy(t => 
            t.CustomAttributes.OfType<PipelineBehaviorPriorityAttribute>().Select(a => a.Priority).FirstOrDefault(uint.MinValue)
        ).ToList();

        foreach (var pipelineBehaviorGenericType in pipelineBehaviorGenericTypes)
        {
            services.AddScoped(typeof(IEventPipelineBehavior<>), pipelineBehaviorGenericType);
        }

        return services;
    }
    
    public static IServiceCollection AddGenericPipeline(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipeline<,>), typeof(GenericPipeline<,>));
        return services;
    }
}

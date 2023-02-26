using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.PipelineBehaviors;
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

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly)
    {
        var applicationServiceTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(false).OfType<ApplicationServiceAttribute>().Any()).ToList();
        foreach (var applicationServiceType in applicationServiceTypes)
        {
            var applicationServiceInterfaceTypes = applicationServiceType.GetCustomAttributes(false).OfType<ApplicationServiceAttribute>().Select(a => a.ServiceType);
            foreach (var applicationServiceInterfaceType in applicationServiceInterfaceTypes)
            {
                services.AddScoped(applicationServiceInterfaceType, applicationServiceType);
            }
        }

        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypesTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>))
        ).ToList();

        foreach (var handlerType in handlerTypesTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);

            var firstGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[0];
            var secondGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[1];
            
            if (typeof(IDomainEvent).IsAssignableFrom(firstGenericHandlerTypeArgument) && secondGenericHandlerTypeArgument == typeof(Unit))
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
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
        ).ToList().OrderBy(t => 
            t.CustomAttributes.OfType<PipelineBehaviorPriorityAttribute>().Select(a => a.Priority).FirstOrDefault(uint.MinValue)
        ).ToList();

        foreach (var pipelineBehaviorGenericType in pipelineBehaviorGenericTypes)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), pipelineBehaviorGenericType);
        }

        return services;
    }
    
    public static IServiceCollection AddGenericPipeline(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipeline<,>), typeof(GenericPipeline<,>));
        return services;
    }
}

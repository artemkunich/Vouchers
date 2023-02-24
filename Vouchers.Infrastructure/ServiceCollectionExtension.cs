using Microsoft.Extensions.Configuration;
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
    public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageHelper, MessageHelper>();
        services.AddScoped<IIdentifierProvider<Guid>, GuidIdentifierProvider>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Vouchers")).ToList();
        
        var handlerTypesTypes = assemblies.SelectMany(assembly => 
            assembly.GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>))
            )
        ).ToList();

        var pipelineBehaviorGenericTypes = assemblies.SelectMany(assembly => 
            assembly.GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && t.IsGenericType &&
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
            )
        ).ToList().OrderBy(t => 
            t.CustomAttributes.OfType<PipelineBehaviorPriorityAttribute>().Select(a => a.Priority).FirstOrDefault(uint.MinValue)
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
                
                continue;
            }


            foreach (var pipelineBehaviorGenericType in pipelineBehaviorGenericTypes)
            {
                var pipelineBehaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(firstGenericHandlerTypeArgument, secondGenericHandlerTypeArgument);
                var pipelineBehaviorType = pipelineBehaviorGenericType.MakeGenericType(firstGenericHandlerTypeArgument, secondGenericHandlerTypeArgument);
                services.AddScoped(pipelineBehaviorInterfaceType, pipelineBehaviorType);
            }
            
            
            var pipelineInterfaceType = typeof(IPipeline<,>).MakeGenericType(firstGenericHandlerTypeArgument, secondGenericHandlerTypeArgument);
            var pipelineType = typeof(GenericPipeline<,>).MakeGenericType(firstGenericHandlerTypeArgument, secondGenericHandlerTypeArgument);
            
            services.AddScoped(pipelineInterfaceType, pipelineType);

        }

        return services;
    }
}

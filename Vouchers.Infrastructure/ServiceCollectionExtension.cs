using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.UseCases;
using Vouchers.Primitives;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtension
{
    
    public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageHelper, MessageHelper>();
        
        var genericHandlerTypes = new[] {typeof(IHandler<>), typeof(IHandler<,>)};

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Vouchers"));
        
        var handlerTypes = new List<Type>();
        foreach (var assembly in assemblies)
        {
            handlerTypes.AddRange(assembly.GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
                t.GetInterfaces()
                    .Any(i => i.IsGenericType && genericHandlerTypes.Contains(i.GetGenericTypeDefinition()))
            ));
        }

        foreach (var handlerType in handlerTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && genericHandlerTypes.Contains(i.GetGenericTypeDefinition()));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);

            if (genericHandlerType.GenericTypeArguments.Length == 1 && typeof(IDomainEvent).IsAssignableFrom(genericHandlerType.GenericTypeArguments.First()))
            {
                var genericHandlerTypeArgument = genericHandlerType.GenericTypeArguments.First();
                if (typeof(IDomainEvent).IsAssignableFrom(genericHandlerTypeArgument))
                {
                    var iMessageHandlerType = typeof(IMessageHandler<>).MakeGenericType(genericHandlerTypeArgument);
                    var messageHandlerType = typeof(MessageHandler<>).MakeGenericType(genericHandlerTypeArgument);

                    services.AddScoped(iMessageHandlerType, messageHandlerType);
                }
            }
            
        }

        return services;
    }
}

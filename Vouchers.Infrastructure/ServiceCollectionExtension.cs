using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Infrastructure.InterCommunication;
using Vouchers.Primitives;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageHelper, MessageHelper>();
        services.AddScoped<IIdentifierProvider<Guid>, GuidIdentifierProvider>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        
        var genericHandlerTypes = new[] {typeof(IHandler<>), typeof(IHandler<,>)};

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Vouchers"));
        
        var handlerTypes = assemblies.SelectMany(assembly => 
            assembly.GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
                t.GetInterfaces().Any(i => i.IsGenericType && genericHandlerTypes.Contains(i.GetGenericTypeDefinition()))
            )
        ).ToList();
        
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

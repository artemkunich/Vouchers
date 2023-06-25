﻿using System.Reflection;
using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Akunich.Extensions.Time;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Domains.Application.PipelineBehaviors;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Persistence.InterCommunication.DomainEventHandlers;
using Vouchers.Persistence.InterCommunication.EventPipelineBehaviors;
using Vouchers.Persistence.PipelineBehaviors;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddScoped<IMessageHelper, MessageHelper>()
            .AddGuidIdentifierProvider()
            .AddSystemTimeProvider()
            .AddScoped<IResourceProvider, ResourceProvider>()
            .AddScoped<INotificationDispatcher, NotificationDispatcher>();

    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddScoped<IAppImageService, AppImageService>()
            .AddScoped<IIdentityIdProvider<Guid>, AuthIdentityProvider>();

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
    
    public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypesTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
        ).ToList();

        foreach (var handlerType in handlerTypesTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);
        }

        var eventMapperTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventMapper<,>))
        ).ToList();
        
        foreach (var eventMapperType in eventMapperTypes)
        {
            var genericEventMapperType = eventMapperType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventMapper<,>));
            if(genericEventMapperType is null)
                continue;
            
            services.AddScoped(genericEventMapperType, eventMapperType);

            var firstGenericHandlerTypeArgument = genericEventMapperType.GenericTypeArguments[0];
            var secondGenericHandlerTypeArgument = genericEventMapperType.GenericTypeArguments[1];

            var domainEventHandlerInterfaceType = typeof(INotificationHandler<>).MakeGenericType(firstGenericHandlerTypeArgument);
            var domainEventHandlerType = typeof(NotificationToOutboxMessageHandler<,>).MakeGenericType(firstGenericHandlerTypeArgument, secondGenericHandlerTypeArgument);
            services.AddScoped(domainEventHandlerInterfaceType, domainEventHandlerType);
        }
        
        return services;
    }
    
    public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
        ).ToList();

        foreach (var handlerType in handlerTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);

            var firstGenericHandlerTypeArgument = genericHandlerType.GenericTypeArguments[0];

            var pipelineInterfaceType = typeof(IIntegrationEventPipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);
            var pipelineType = typeof(IntegrationEventPipeline<>).MakeGenericType(firstGenericHandlerTypeArgument);

            services.AddScoped(pipelineInterfaceType, pipelineType);
        }
        
        
        services.AddScoped<IMessagesProcessor,MessagesProcessor>();
        MessagesProcessor.AddEventTypesFromAssembly(assembly);
        return services;
    }
    
    public static IServiceCollection AddRequestPipelineBehaviors(this IServiceCollection services, Assembly assembly) =>
        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(DbUpdateConcurrencyExceptionBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(DbSaveChangesBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ErrorsCultureInfo<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(IdentityRegistrationBehavior<,>));

    public static IServiceCollection AddEventPipelineBehaviors(this IServiceCollection services, Assembly assembly) =>
        services
            .AddScoped(typeof(IIntegrationEventPipelineBehavior<>), typeof(DbUpdateExceptionBehavior<>))
            .AddScoped(typeof(IIntegrationEventPipelineBehavior<>), typeof(DbUpdateConcurrencyExceptionBehavior<>))
            .AddScoped(typeof(IIntegrationEventPipelineBehavior<>), typeof(TestIntegrationEventPipelineBehavior<>));
    
    public static IServiceCollection AddGenericPipeline(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IPipeline<,>), typeof(GenericPipeline<,>));
    
}
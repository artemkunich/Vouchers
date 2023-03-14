using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Vouchers.Application.Infrastructure;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Primitives;
using Vouchers.Persistence.Repositories;


namespace Vouchers.Persistence;
    
public static class TypeExtensions
{
    public static bool IsInheritedFromGeneric(this Type type, Type genericType)
    {
        var currentBaseType = type.BaseType;

        while (currentBaseType != null)
        {
            if (currentBaseType.IsGenericType && currentBaseType.GetGenericTypeDefinition() == genericType)
                return true;
            
            currentBaseType = currentBaseType.BaseType;
        }

        return false;
    }
}
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositoryForEntities(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IMessageDataSerializer, MessageDataSerializer>();
        
        var entityTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract &&
            t.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntity<>))
        ).ToList();

        var executingAssembly = Assembly.GetExecutingAssembly();
        
        foreach (var entityType in entityTypes)
        {
            var idType = entityType.GetProperty(nameof(Entity<object>.Id))?.PropertyType;

            if(idType is null)
                continue;
            
            var genericReadOnlyRepositoryType = typeof(IReadOnlyRepository<,>).MakeGenericType(entityType, idType);
            var readOnlyRepositoryType = executingAssembly.GetTypes()
                .Where(t => genericReadOnlyRepositoryType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .FirstOrDefault(typeof(GenericReadOnlyRepository<,>).MakeGenericType(entityType, idType));
            
            services.AddScoped(genericReadOnlyRepositoryType, readOnlyRepositoryType);

            if (!entityType.IsInheritedFromGeneric(typeof(AggregateRoot<>)))
                continue;
            
            var genericRepositoryType = typeof(IRepository<,>).MakeGenericType(entityType, idType);
            var repositoryType = executingAssembly.GetTypes()
                .Where(t => genericRepositoryType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .FirstOrDefault(typeof(GenericRepository<,>).MakeGenericType(entityType, idType));
            
            services.AddScoped(genericRepositoryType, repositoryType);
        }

        return services;
    }
}

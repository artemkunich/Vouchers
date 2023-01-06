using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Vouchers.Application.Infrastructure;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Application.UseCases;
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
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageDataSerializer, MessageDataSerializer>();
        
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).Where(x => x.FullName != null && x.FullName.StartsWith("Vouchers")).ToList();
        assemblies.Add(Assembly.GetExecutingAssembly());
        
        var aggregateRootTypes = assemblies.SelectMany(assembly =>
            assembly.GetTypes().Where(t =>
                t.IsClass && !t.IsAbstract &&
                t.GetInterfaces()
                    .Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntity<>)) &&
                t.IsInheritedFromGeneric(typeof(AggregateRoot<>))
            )).ToList();

        var executingAssembly = Assembly.GetExecutingAssembly();
        
        foreach (var aggregateRootType in aggregateRootTypes)
        {
            var idType = aggregateRootType.GetProperty(nameof(Entity<object>.Id))?.PropertyType;

            if(idType is null)
                continue;
            
            var genericRepositoryType = typeof(IRepository<,>).MakeGenericType(aggregateRootType, idType);
            var genericRepositoryTypeInfo = genericRepositoryType.GetTypeInfo();

            var repositoryType = executingAssembly.GetTypes()
                .Where(t => genericRepositoryTypeInfo.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .FirstOrDefault(typeof(GenericRepository<,>).MakeGenericType(aggregateRootType, idType));
            
            services.AddScoped(genericRepositoryType, repositoryType);
        }

        return services;
    }
}

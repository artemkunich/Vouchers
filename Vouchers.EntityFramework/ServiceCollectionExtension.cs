using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Entities;
using Vouchers.EntityFramework.Repositories;


namespace Vouchers.EntityFramework;
    
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        var assemblies = assemblyNames.Where(x=> x.FullName.StartsWith("Vouchers")).Select(Assembly.Load);
        
        var aggregateRootTypes = new List<Type>();
        foreach (var assembly in assemblies)
        {
            aggregateRootTypes.AddRange(assembly.GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && 
                t.BaseType != null && t.BaseType.IsGenericType && 
                t.BaseType.GetGenericTypeDefinition() == typeof(Entity<>) && 
                Attribute.GetCustomAttribute(t, typeof(AggregateRootAttribute)) != null
            ));
        }

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

    public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        var genericHandlerTypes = new[] {typeof(IHandler<>), typeof(IHandler<,>)};

        var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        var assemblies = assemblyNames.Where(x=> x.FullName.StartsWith("Vouchers")).Select(Assembly.Load).ToList();
        assemblies.Add(Assembly.GetExecutingAssembly());
            
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
        }

        return services;
    }
}

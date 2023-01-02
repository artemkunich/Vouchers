using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Entities;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.InterCommunication;
using Vouchers.Values;

namespace Vouchers.EntityFramework;
    
public static class IServiceCollectionExtension
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

    /*public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IRepository<AccountItem,Guid>, AccountItemRepository>()
            .AddScoped<IRepository<HolderTransactionRequest,Guid>, HolderTransactionRequestRepository>()
            .AddScoped<IRepository<DomainAccount,Guid>, DomainAccountRepository>()
            .AddScoped<IRepository<DomainContract,Guid>, DomainContractRepository>()
            .AddScoped<IRepository<DomainOffersPerIdentityCounter,Guid>, DomainOffersPerIdentityCounterRepository>()
            .AddScoped<IRepository<Domain,Guid>, DomainRepository>()
            .AddScoped<IRepository<Login,Guid>, LoginRepository>()
            .AddScoped<IRepository<Unit,Guid>, UnitRepository>()
            .AddScoped<IRepository<UnitType,Guid>, UnitTypeRepository>()
            
            .AddScoped<IRepository<Account,Guid>, GenericRepository<Account,Guid>>()
            .AddScoped<IRepository<HolderTransaction,Guid>, GenericRepository<HolderTransaction,Guid>>()
            .AddScoped<IRepository<HolderTransactionItem,Guid>, GenericRepository<HolderTransactionItem,Guid>>()
            .AddScoped<IRepository<IssuerTransaction,Guid>, GenericRepository<IssuerTransaction,Guid>>()
            .AddScoped<IRepository<DomainOffer,Guid>, GenericRepository<DomainOffer,Guid>>()
            .AddScoped<IRepository<VoucherValue,Guid>, GenericRepository<VoucherValue,Guid>>()
            .AddScoped<IRepository<Identity,Guid>, GenericRepository<Identity,Guid>>()
            .AddScoped<IRepository<CroppedImage,Guid>, GenericRepository<CroppedImage,Guid>>()
            
            .AddScoped<IRepository<OutboxMessage,Guid>, GenericRepository<OutboxMessage,Guid>>()
            .AddScoped<IRepository<InboxMessage,Guid>, GenericRepository<InboxMessage,Guid>>();*/

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
                !t.IsAbstract && !t.IsInterface &&
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

    /*public static IServiceCollection AddQueryHandlers(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddIdentityHandlers()
            .AddDomainOfferHandlers()
            .AddDomainHandlers()
            .AddDomainAccountHandlers()
            .AddIssuerValueHandlers()
            .AddIssuerVoucherHandlers()
            .AddIssuerTransactionHandlers()
            .AddHolderValueHandlers()
            .AddHolderTransactionHandlers()
            .AddHolderVoucherHandlers()
            .AddHolderTransactionRequestHandlers();*/

    private static IServiceCollection AddIdentityHandlers(this IServiceCollection services) =>
        services
            .AddHandler<string, Guid?, IdentityQueryHandler>()
            .AddHandler<Guid?, IdentityDetailDto, IdentityDetailQueryHandler>();

    private static IServiceCollection AddDomainOfferHandlers(this IServiceCollection services) =>
        services
            .AddHandler<DomainOffersQuery, IEnumerable<DomainOfferDto>, DomainOffersQueryHandler>()
            .AddHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>, IdentityDomainOffersQueryHandler>();

    private static IServiceCollection AddDomainHandlers(this IServiceCollection services) =>
        services
            .AddHandler<DomainsQuery, IEnumerable<DomainDto>, DomainsQueryHandler>()
            .AddHandler<Guid, DomainDetailDto, DomainDetailQueryHandler>();

    private static IServiceCollection AddDomainAccountHandlers(this IServiceCollection services) =>
        services
            .AddHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>, DomainAccountsQueryHandler>()
            .AddHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>, IdentityDomainAccountsQueryHandler>();

    private static IServiceCollection AddIssuerValueHandlers(this IServiceCollection services) => 
        services
            .AddHandler<IssuerValuesQuery, IEnumerable<VoucherValueDto>, IssuerValuesQueryHandler>()
            .AddHandler<Guid, VoucherValueDetailDto, VoucherValueDetailQueryHandler>();

    private static IServiceCollection AddIssuerVoucherHandlers(this IServiceCollection services) =>
        services.AddHandler<IssuerVouchersQuery, IEnumerable<VoucherDto>, IssuerVouchersQueryHandler>();

    private static IServiceCollection AddIssuerTransactionHandlers(this IServiceCollection services) =>
        services.AddHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>, IssuerTransactionsQueryHandler>();

    private static IServiceCollection AddHolderValueHandlers(this IServiceCollection services) =>
        services.AddHandler<HolderValuesQuery, IEnumerable<VoucherValueDto>, HolderValuesQueryHandler>();
    

    private static IServiceCollection AddHolderTransactionHandlers(this IServiceCollection services) =>
        services.AddHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>, HolderTransactionsQueryHandler>();
    

    private static IServiceCollection AddHolderVoucherHandlers(this IServiceCollection services) =>
        services.AddHandler<HolderVouchersQuery, IEnumerable<VoucherDto>, HolderVouchersQueryHandler>();

    private static IServiceCollection AddHolderTransactionRequestHandlers(this IServiceCollection services) =>
        services
            .AddHandler<HolderTransactionRequestsQuery, IEnumerable<HolderTransactionRequestDto>, HolderTransactionRequestsQueryHandler>()
            .AddHandler<DomainValuesQuery, IEnumerable<VoucherValueDto>, DomainValuesQueryHandler>()
            .AddHandler<Guid, HolderTransactionRequestDto, HolderTransactionRequestQueryHandler>();

    private static IServiceCollection AddHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq, TRes> =>
        services.AddScoped<IHandler<TReq, TRes>, THandler>();
}

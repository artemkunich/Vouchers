using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Time;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Core.Application.UseCases.UnitTypeCases;
using Vouchers.Core.Infrastructure;
using Vouchers.Domains.Application.UseCases.VoucherValueCases;
using Vouchers.Domains.Infrastructure;
using Vouchers.Files.Infrastructure;
using Vouchers.Persistence;
using Vouchers.Queries.Infrastructure;
using Vouchers.Root.NotificationExtensions;

namespace Vouchers.Root;
    
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVouchers(this IServiceCollection services) => services
        .AddInfrastructureServices()
        .AddModules()
        .BindNotifications()
        .AddRequestDispatcher()
        .AddNotificationDispatcher();

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services) => services
        .AddGuidIdentifierProvider()
        .AddSystemTimeProvider()
        .AddGenericRepositories();

    private static IServiceCollection AddModules(this IServiceCollection services) => services
        .AddCoreModule()
        .AddDomainsModule()
        .AddFilesModule()
        .AddQueriesModule();
    
    private static IServiceCollection BindNotifications(this IServiceCollection services) => services
        .BindNotification<UnitTypeDeletedNotification, DeleteVoucherValueCommand>(n => n.ToDeleteVoucherValueCommand());
    
}

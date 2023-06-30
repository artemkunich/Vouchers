using Akunich.Application.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Time;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Core.Infrastructure;
using Vouchers.Domains.Infrastructure;
using Vouchers.Files.Infrastructure;
using Vouchers.Persistence;
using Vouchers.Queries.Infrastructure;

namespace Vouchers.Infrastructure;
    
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVouchers(this IServiceCollection services) => services
        .AddInfrastructureServices()
        .AddModules()
        .AddScoped<IRequestDispatcher, RequestDispatcher>()
        .AddScoped<INotificationDispatcher, NotificationDispatcher>()
        .AddApplication(typeof(ServiceCollectionExtension).Assembly);
    
    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services) => services
        .AddGuidIdentifierProvider()
        .AddSystemTimeProvider()
        .AddGenericRepositories();

    private static IServiceCollection AddModules(this IServiceCollection services) => services
        .AddCoreModule()
        .AddDomainsModule()
        .AddFilesModule()
        .AddQueriesModule();
}

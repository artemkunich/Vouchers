using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.ServiceProviders;
using Vouchers.Application.Services;

namespace Vouchers.Application.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configurations) =>
        services
            .AddScoped<IAuthIdentityProvider, AuthIdentityProvider>()
            .AddScoped<IAppImageService, AppImageService>();
}
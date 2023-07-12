using Akunich.Application.Abstractions;
using Akunich.Extensions.ResourceRoles;
using Microsoft.Extensions.DependencyInjection;

namespace Vouchers.Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreModule(this IServiceCollection serviceCollection) => serviceCollection
        .AddApplication(typeof(Application.UseCases.AccountCases.CreateAccountCommandHandler).Assembly)
        .AddResourceRolesProvider()
        .SetDefaultRoles("User");
}
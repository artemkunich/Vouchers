using Akunich.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Vouchers.Domains.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainsModule(this IServiceCollection serviceCollection) =>
        serviceCollection.AddApplication(typeof(Application.UseCases.DomainCases.CreateDomainCommand).Assembly);
}
using Akunich.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Vouchers.Queries.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueriesModule(this IServiceCollection serviceCollection) =>
        serviceCollection.AddApplication(typeof(ServiceCollectionExtensions).Assembly);
}
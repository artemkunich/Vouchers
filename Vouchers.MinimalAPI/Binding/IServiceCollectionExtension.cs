using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.UseCases.DomainCases;
using Vouchers.Domains.Application.UseCases.IdentityCases;

namespace Vouchers.MinimalAPI.Binding;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddFormParameterProviders(this IServiceCollection services) =>
        services
            .AddScoped<IFormParameterProvider<CropParametersDto>, CropParametersProvider>()
            .AddScoped<IFormParameterProvider<CreateIdentityCommand>, CreateIdentityCommandProvider>()
            .AddScoped<IFormParameterProvider<UpdateIdentityCommand>, UpdateIdentityCommandProvider>()
            .AddScoped<IFormParameterProvider<UpdateDomainDetailCommand>, UpdateDomainDetailCommandProvider>();
}
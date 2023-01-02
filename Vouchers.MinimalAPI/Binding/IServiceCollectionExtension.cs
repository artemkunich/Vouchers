using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;

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
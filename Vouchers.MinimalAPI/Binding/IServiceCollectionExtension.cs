using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;

namespace Vouchers.MinimalAPI.Binding;

public static class IServiceCollectionExtension
{
    public static void AddFormParameterProviders(this IServiceCollection services)
    {         
        services.AddScoped<IFormParameterProvider<CropParametersDto>, CropParametersProvider>();
        services.AddScoped<IFormParameterProvider<CreateIdentityCommand>, CreateIdentityCommandProvider>();
        services.AddScoped<IFormParameterProvider<UpdateIdentityCommand>, UpdateIdentityCommandProvider>();
        services.AddScoped<IFormParameterProvider<UpdateDomainDetailCommand>, UpdateDomainDetailCommandProvider>();
    }
}
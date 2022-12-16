using Vouchers.Application.Commands.DomainCommands;

namespace Vouchers.MinimalAPI.Validation;

public static class IServiceCollectionExtension
{
    public static void AddFormValidators(this IServiceCollection services)
    {         
        services.AddSingleton<IFormValidator<UpdateDomainDetailCommand>, UpdateDomainDetailComandValidator>();
    }
}
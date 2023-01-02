using Vouchers.Application.Commands.DomainCommands;

namespace Vouchers.MinimalAPI.Validation;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddFormValidators(this IServiceCollection services) =>
        services.AddSingleton<IFormValidator<UpdateDomainDetailCommand>, UpdateDomainDetailComandValidator>();
}
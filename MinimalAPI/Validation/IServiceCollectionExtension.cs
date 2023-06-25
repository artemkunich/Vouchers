using Vouchers.Domains.Application.UseCases.DomainCases;

namespace Vouchers.MinimalAPI.Validation;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddFormValidators(this IServiceCollection services) =>
        services.AddSingleton<IFormValidator<UpdateDomainDetailCommand>, UpdateDomainDetailComandValidator>();
}
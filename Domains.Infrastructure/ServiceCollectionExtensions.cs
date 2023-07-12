using Akunich.Application.Abstractions;
using Akunich.Extensions.ResourceRoles;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Domains.Application.UseCases.DomainOfferCases;
using Vouchers.Domains.Application.UseCases.IdentityCases;

namespace Vouchers.Domains.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static string[] IdentityCommandRoles => new[] { IdentityRoles.User, IdentityRoles.Manager, IdentityRoles.Admin };
    
    public static string[] DomainOfferCommandRoles => new[] { IdentityRoles.Manager };
    
    public static IServiceCollection AddDomainsModule(this IServiceCollection serviceCollection) => serviceCollection
        .AddApplication(typeof(Application.UseCases.DomainCases.CreateDomainCommand).Assembly)
        .SetRoles<CreateDomainOfferCommand>(DomainOfferCommandRoles)
        .SetRoles<UpdateDomainOfferCommand>(DomainOfferCommandRoles)
        .SetRoles<CreateIdentityCommand>(IdentityCommandRoles)
        .SetRoles<UpdateIdentityCommand>(IdentityCommandRoles)
        .SetRoles<DeleteIdentityCommand>(IdentityCommandRoles);
}
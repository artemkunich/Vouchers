using FluentValidation;
using Vouchers.Domains.Application.UseCases.DomainCases;

namespace Vouchers.MinimalAPI.Validation;

internal class DomainAccountsQueryValidator: AbstractValidator<UpdateDomainDetailCommand>
{
    public DomainAccountsQueryValidator()
    {
        RuleFor(x => x.DomainId).NotEmpty();
    }
}
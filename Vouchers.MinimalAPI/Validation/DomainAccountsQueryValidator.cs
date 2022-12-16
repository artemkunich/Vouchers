using FluentValidation;
using Vouchers.Application.Commands.DomainCommands;

namespace Vouchers.MinimalAPI.Validation;

internal class DomainAccountsQueryValidator: AbstractValidator<UpdateDomainDetailCommand>
{
    public DomainAccountsQueryValidator()
    {
        RuleFor(x => x.DomainId).NotEmpty();
    }
}
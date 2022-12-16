using FluentValidation.Results;
using Vouchers.Application.Commands.DomainCommands;

namespace Vouchers.MinimalAPI.Validation;

internal class UpdateDomainDetailComandValidator : IFormValidator<UpdateDomainDetailCommand>
{
    public ValidationResult Validate(IFormCollection formCollection)
    {
        var validationResult = new ValidationResult();

        if (string.IsNullOrEmpty(formCollection["domainId"]))
        {
            validationResult.Errors.Add(new ValidationFailure("domainId","is missing"));
            return validationResult;
        }

        if (!Guid.TryParse(formCollection["domainId"], out var domainId))
        {
            validationResult.Errors.Add(new ValidationFailure("domainId","is not guid"));
            return validationResult;
        }
            
        
        if(domainId == Guid.Empty)
            validationResult.Errors.Add(new ValidationFailure("domainId","is empty guid"));

        return validationResult;
    }
}
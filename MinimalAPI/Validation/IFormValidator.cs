using FluentValidation.Results;

namespace Vouchers.MinimalAPI.Validation;

internal interface IFormValidator<T> where T: class
{
    public ValidationResult Validate(IFormCollection formCollection);
}
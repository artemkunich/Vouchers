using FluentValidation;
using Vouchers.MinimalAPI.Validation;

namespace Vouchers.MinimalAPI.Filters;

internal class FormValidatorFilter<T>: IEndpointFilter where T: class
{
    private readonly IFormValidator<T> _validator;

    public FormValidatorFilter(IFormValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var form = context.HttpContext.Request.Form;
        
        var validationResult = _validator.Validate(form);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(new { Errors = validationResult.Errors.Select(x => x.ErrorMessage)});
        }
        
        var result = await next(context);
        
        return result;
    }
}
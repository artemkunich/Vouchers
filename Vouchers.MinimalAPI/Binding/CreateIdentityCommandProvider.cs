using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;

namespace Vouchers.MinimalAPI.Binding;

public class CreateIdentityCommandProvider : IFormParameterProvider<CreateIdentityCommand>
{
    readonly IHttpContextAccessor _ctxAccessor;
    readonly IFormParameterProvider<CropParametersDto> _formToCropParametersParameterProvider;

    public CreateIdentityCommandProvider(IHttpContextAccessor ctxAccessor, IFormParameterProvider<CropParametersDto> formToCropParametersParameterProvider)
    {
        _ctxAccessor = ctxAccessor;
        _formToCropParametersParameterProvider = formToCropParametersParameterProvider;
    }

    public CreateIdentityCommand GetParameter()
    {
        var form = _ctxAccessor.HttpContext.Request.Form;
        
        return new()
        {
            FirstName = form["firstName"],
            LastName = form["lastName"],
            Email = form["email"],
            Image = form.Files.FirstOrDefault(),
            CropParameters = _formToCropParametersParameterProvider.GetParameter()
        }; 
    }
        
}
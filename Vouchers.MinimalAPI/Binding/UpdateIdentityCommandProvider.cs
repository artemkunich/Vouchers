using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.UseCases.IdentityCases;

namespace Vouchers.MinimalAPI.Binding;

public class UpdateIdentityCommandProvider : IFormParameterProvider<UpdateIdentityCommand>
{
    readonly IHttpContextAccessor _ctxAccessor;
    readonly IFormParameterProvider<CropParametersDto> _formToCropParametersParameterProvider;

    public UpdateIdentityCommandProvider(IHttpContextAccessor ctxAccessor, IFormParameterProvider<CropParametersDto> formToCropParametersParameterProvider)
    {
        _ctxAccessor = ctxAccessor;
        _formToCropParametersParameterProvider = formToCropParametersParameterProvider;
    }

    public UpdateIdentityCommand GetParameter()
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
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Dtos;

namespace Vouchers.MinimalAPI.Binding;

public class UpdateDomainDetailCommandProvider : IFormParameterProvider<UpdateDomainDetailCommand>
{
    readonly IHttpContextAccessor _ctxAccessor;
    readonly IFormParameterProvider<CropParametersDto> _formToCropParametersParameterProvider;

    public UpdateDomainDetailCommandProvider(IHttpContextAccessor ctxAccessor, IFormParameterProvider<CropParametersDto> formToCropParametersParameterProvider)
    {
        _ctxAccessor = ctxAccessor;
        _formToCropParametersParameterProvider = formToCropParametersParameterProvider;
    }
    
    public UpdateDomainDetailCommand GetParameter()
    {
        var form = _ctxAccessor.HttpContext.Request.Form;
        
        var domainId = Guid.Parse(form["domainId"]);
        bool? isPublic = string.IsNullOrEmpty(form["isPublic"]) ? null : bool.Parse(form["isPublic"]);
        return new UpdateDomainDetailCommand
        {
            DomainId = domainId,
            Name = form["name"],
            Description = form["description"],
            IsPublic = isPublic,
            Image = form.Files.FirstOrDefault(),
            CropParameters = _formToCropParametersParameterProvider.GetParameter()
        };
    }
}
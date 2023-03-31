using Vouchers.Domains.Application.Dtos;

namespace Vouchers.MinimalAPI.Binding;

public class CropParametersProvider : IFormParameterProvider<CropParametersDto>
{
    readonly IHttpContextAccessor _ctxAccessor;

    public CropParametersProvider(IHttpContextAccessor ctxAccessor)
    {
        _ctxAccessor = ctxAccessor;
    }

    public CropParametersDto GetParameter()
    {
        var form = _ctxAccessor.HttpContext.Request.Form;
        
        return decimal.TryParse(form["cropParameters.x"], out var x)
            && decimal.TryParse(form["cropParameters.y"], out var y)
            && decimal.TryParse(form["cropParameters.width"], out var width)
            && decimal.TryParse(form["cropParameters.height"], out var height)
                ? new CropParametersDto
                {
                    X = x,
                    Y = y,
                    Width = width,
                    Height = height,
                }
                : null;
    }
}
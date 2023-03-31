using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vouchers.Domains.Application.Errors;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Errors;

namespace Vouchers.API.Services;

public static class ControllerExtension
{
    public static IActionResult FromResult<TValue>(this Controller controller, Result<TValue> result)
    {
        if (result.IsFailure)
        {
            if (result.Errors.OfType<NotRegisteredError>().Any())
                return controller.NotFound();
                
            return controller.BadRequest(result.Errors);
        }
           
        if (typeof(TValue) == typeof(Unit))
            return controller.NoContent();
        
        if (result.Value is null)
            return controller.NotFound();
            
        return controller.Json(result.Value);
    }
}
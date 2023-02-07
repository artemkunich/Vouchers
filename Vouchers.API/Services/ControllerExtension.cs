using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vouchers.Application;

namespace Vouchers.API.Services;

public static class ControllerExtension
{
    public static IActionResult FromResult(this Controller controller, Result result)
    {
        if (result.IsFailure)
        {
            if (result.Errors.First() == Error.NotRegistered())
                return controller.NotFound();
                
            return controller.BadRequest(result.Errors);
        }
        
        if (result.IsSuccess)
            return controller.NoContent();

        return controller.BadRequest(result.Errors);
    }
    
    public static IActionResult FromResult<TValue>(this Controller controller, Result<TValue> result)
    {
        if (result.IsFailure)
        {
            if (result.Errors.First() == Error.NotRegistered())
                return controller.NotFound();
                
            return controller.BadRequest(result.Errors);
        }
            
        
        if (result.Value is null)
            return controller.NotFound();
            
        return controller.Json(result.Value);
    }
}
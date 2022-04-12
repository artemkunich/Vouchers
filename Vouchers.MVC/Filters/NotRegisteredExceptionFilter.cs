using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Vouchers.MVC.Filters
{
    public class NotRegisteredExceptionFilter: IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "IdentityDetail" },
                    { "action", "Index" }
                });

            return Task.CompletedTask;
        }
    }
}

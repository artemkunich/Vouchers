using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;

namespace Vouchers.API.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Authorize(Roles = "User")]
    public class ValueDetailController : Controller
    {
        private readonly IDispatcher dispatcher;

        public ValueDetailController(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("[controller]/{valueId:guid}")]
        public async Task<IActionResult> Get(Guid valueId)
        {
            var result = await dispatcher.DispatchAsync<Guid, VoucherValueDetailDto>(valueId);
            if(result is null)
                return NotFound();

            return Json(result);
        }
    }
}

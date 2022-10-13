using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;

namespace Vouchers.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    public sealed class HolderTransactionRequestController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public HolderTransactionRequestController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("[controller]/{transactionRequestId:guid}")]
        public async Task<IActionResult> Get(Guid transactionRequestId)
        {
            var holderTransactionRequest = await _dispatcher.DispatchAsync<Guid, HolderTransactionRequestDto>(transactionRequestId);
            if(holderTransactionRequest is null)
                return NotFound();

            return Json(holderTransactionRequest);
        }
    }
}

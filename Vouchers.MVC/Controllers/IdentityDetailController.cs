using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.UseCases;
using Vouchers.MVC.Extensions;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize]
    public class IdentityDetailController : Controller
    {
        private readonly IRequestDispatcher dispatcher;

        public IdentityDetailController(IRequestDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? identityId)
        {
            var detail = await dispatcher.DispatchAsync<Guid?, IdentityDetailDto>(identityId);
            return View(detail);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(IdentityDetailDto identityDetailDto)
        {
            var loginName = User.Identity.Name;
            var identityId = await dispatcher.DispatchAsync<string, Guid?>(loginName);

            if (identityId is null)
            {
                await dispatcher.DispatchAsync(new CreateIdentityCommand { LoginName = loginName, IdentityDetailDto = identityDetailDto });
            }
            else
            {
                await dispatcher.DispatchAsync(new UpdateIdentityDetailCommand { IdentityDetailDto = identityDetailDto });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MVC.Extensions;
using Vouchers.MVC.Models;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoginsController : Controller
    {
        private readonly IRequestDispatcher dispatcher;
        public LoginsController(IRequestDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Index(LoginsQuery query)
        {
            var logins = await dispatcher.DispatchAsync<LoginsQuery, IPaginatedEnumerable<LoginDto>>(query);

            var model = new LoginsViewModel { Items = logins };

            model.OrderBy = query.OrderBy;
            switch (query.OrderBy)         
            {
                case "LoginName":
                    model.LoginNameOrder = "Desc";
                    break;
                case "LoginNameDesc":
                    model.LoginNameOrder = "";
                    break;
                case "IdentityName":
                    model.IdentityNameOrder = "Desc";
                    break;
                case "IdentityNameDesc":
                    model.IdentityNameOrder = "";
                    break;
                case "FirstName":
                    model.FirstNameOrder = "Desc";
                    break;
                case "FirstNameDesc":
                    model.FirstNameOrder = "";
                    break;
                case "LastName":
                    model.LastNameOrder = "Desc";
                    break;
                case "LastNameDesc":
                    model.LastNameOrder = "";
                    break;
            };

            model.LoginNameFilter = query.LoginName;
            model.IdentityNameFilter = query.IdentityName;
            model.FirstNameFilter = query.FirstName;
            model.LastNameFilter = query.LastName;
            return View(model);

        }
        
        [HttpPost]
        public async Task<IActionResult> Index(IdentityDetailDto identityDetailDto)
        {
            var loginName = User.Identity.Name;
            var identityId = await dispatcher.DispatchAsync<string, Guid?>(loginName);
            if (identityId is null)
                await dispatcher.DispatchAsync(new CreateIdentityCommand { LoginName = loginName, IdentityDetailDto = identityDetailDto });
            else
                await dispatcher.DispatchAsync(new UpdateIdentityDetailCommand {IdentityDetailDto = identityDetailDto });

            return RedirectToAction("Index", "Home");
        }
    }
}

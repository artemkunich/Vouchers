using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.MVC.Models;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestDispatcher dispatcher;


        public HomeController(ILogger<HomeController> logger, IRequestDispatcher dispatcher)
        {
            _logger = logger;
            this.dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index()
        {
            var identityId = await dispatcher.DispatchAsync<string, Guid?>(User.Identity.Name);
            if (identityId == null)
                return RedirectToAction("Index", "IdentityDetail");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

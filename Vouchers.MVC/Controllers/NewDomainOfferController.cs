using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize(Roles = "manager")]
    public class NewDomainOfferController : Controller
    {
        private readonly IRequestDispatcher dispatcher;

        public NewDomainOfferController(IRequestDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

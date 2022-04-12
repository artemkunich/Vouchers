using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.MVC.Models;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize]
    public class DomainOffersController : Controller
    {
        private readonly IRequestDispatcher dispatcher;

        public DomainOffersController(IRequestDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index(DomainOffersQuery query)
        {           
            var domainOffers = await dispatcher.DispatchAsync<DomainOffersQuery, IPaginatedEnumerable<DomainOfferDto>>(query);

            var model = new DomainOffersViewModel { Items = domainOffers };

            return View(model);
        }
    }
}

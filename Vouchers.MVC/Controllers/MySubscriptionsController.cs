using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.MVC.Extensions;
using Vouchers.MVC.Filters;
using Vouchers.MVC.Services;

namespace Vouchers.MVC.Controllers
{
    [Authorize]
    [TypeFilter(typeof(NotRegisteredExceptionFilter))]
    public class MySubscriptionsController : Controller
    {
        private readonly IRequestDispatcher dispatcher;

        public MySubscriptionsController(IRequestDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var result = await dispatcher.DispatchAsync<SubscriptionsQuery, IEnumerable<SubscriptionDto>>(new SubscriptionsQuery { });
            return View(result);
        }

        [HttpPost]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DomainsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DomainsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DomainsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DomainsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DomainsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DomainsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Auth;
using Microsoft.Extensions.Logging;
using Vouchers.MVC.Areas.Identity.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Vouchers.MVC.Extensions;
using Vouchers.MVC.Controllers;

namespace Vouchers.MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("Identity/Account/Login")]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IEmailSender _emailSender;

        public LoginController(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginViewModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    //    return Redirect(model.ReturnUrl);


                    return RedirectToAction(nameof(MySubscriptionsController.Index), nameof(MySubscriptionsController).ToControllerNameOnly(), new { area = "" });
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid login attempt");
            return View("../LoginView/Index", model);
        }
    }
}

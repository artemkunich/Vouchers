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

namespace Vouchers.MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("Identity/Account/Login")]
    public class LoginViewController : Controller
    {
        [HttpGet]
        public  IActionResult Index(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var model = new LoginViewModel { ReturnUrl = returnUrl }; 
            return View(model);
        }
    }
}

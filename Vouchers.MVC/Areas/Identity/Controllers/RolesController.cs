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
using Microsoft.AspNetCore.Authorization;

namespace Vouchers.MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("Identity/Roles")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(
            ILogger<LoginViewModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);
            var roles = await _userManager.GetRolesAsync(user);
            return Json(
                new { 
                    IsUser = roles.Contains("User"),
                    IsManager = roles.Contains("Manager"),
                    IsAdmin = roles.Contains("Admin")
                }    
            );
        }

        [HttpPost]
        public async Task<IActionResult> Index(string loginName, bool isUser, bool isManager, bool isAdmin)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (isUser)
            {
                if (!roles.Contains("User"))
                    await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                if (roles.Contains("User"))
                    await _userManager.RemoveFromRoleAsync(user, "User");
            }

            if (isManager)
            {
                if (!roles.Contains("Manager"))
                    await _userManager.AddToRoleAsync(user, "Manager");
            }
            else
            {
                if (roles.Contains("Manager"))
                    await _userManager.RemoveFromRoleAsync(user, "Manager");
            }

            if (isAdmin)
            {
                if (!roles.Contains("Admin"))
                    await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                if (roles.Contains("Admin"))
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            
            return CreatedAtAction( nameof(Index), new { loginName = loginName });
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouchers.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServerHost.Quickstart.UI
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _dbContext;

        public AdminController(
            ILogger<LoginViewModel> logger,
            UserManager<ApplicationUser> userManager,
            AuthDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Users(string loginName, int? pageSize = 100, int? pageIndex = 0)
        {

            var users = _dbContext.Users.AsQueryable();
            if (!string.IsNullOrEmpty(loginName))
                users = users.Where(u => u.UserName.Contains(loginName));

            var usersList = users.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value).ToList();
            var userList = new List<UserViewModel>();

            foreach (var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserViewModel() 
                {                    
                    LoginName = user.UserName,
                    IsUser = roles.Contains("User"),
                    IsManager = roles.Contains("Manager"),
                    IsAdmin = roles.Contains("Admin")
                });
            }

            return View(new UsersViewModel()
            {
                LoginNameFilter = loginName,
                Users = userList
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddUserRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("User"))
                await _userManager.AddToRoleAsync(user, "User");

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("User"))
                await _userManager.RemoveFromRoleAsync(user, "User");

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> AddManagerRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Manager"))
                await _userManager.AddToRoleAsync(user, "Manager");

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveManagerRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Manager"))
                await _userManager.RemoveFromRoleAsync(user, "Manager");

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> AddAdminRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveAdminRole(string loginName)
        {
            var user = await _userManager.FindByEmailAsync(loginName);

            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
                await _userManager.RemoveFromRoleAsync(user, "Admin");

            return NoContent();
        }
    }
}

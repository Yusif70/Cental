using Cental.Models;
using Cental.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                    if (user is null)
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                        return View(model);
                    }
                }
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(model);
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Info(string userName)
        {
            AppUser? user = await _userManager.FindByNameAsync(userName);
            return View(user);
        }
        //public async Task<IActionResult> AddRole()
        //{

        //    var role1 = new IdentityRole()
        //    {
        //        Name = "SuperAdmin",
        //    };
        //    var role2 = new IdentityRole()
        //    {
        //        Name = "Admin",
        //    };
        //    var role3 = new IdentityRole()
        //    {
        //        Name = "User",
        //    };

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);
        //    return Json("Yarandi");
        //}

        //public async Task<IActionResult> AddAdmin()
        //{
        //    var user = new AppUser()
        //    {
        //        Name = "SuperAdmin",
        //        Surname = "SuperAdmin",
        //        UserName = "SuperAdmin",
        //        Email = "superadmin@gmail.com",
        //        EmailConfirmed = true
        //    };
        //    await _userManager.CreateAsync(user, "SuperAdmin123.");
        //    await _userManager.AddToRolesAsync(user, ["SuperAdmin", "Admin"]);
        //    return Json(user);
        //}
    }
}

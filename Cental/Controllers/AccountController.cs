using Cental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register registerModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.Username,
                    Name = registerModel.Name,
                    Surname = registerModel.Surname
                };

                IdentityResult identityResults = await _userManager.CreateAsync(appUser, registerModel.Password);

                if (!identityResults.Succeeded)
                {
                    foreach (IdentityError error in identityResults.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(registerModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(registerModel);
        }
    }
}

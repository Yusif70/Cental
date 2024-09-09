using Cental.Models;
using Cental.Models.Account;
using Cental.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Email = model.Email,
                    UserName = model.Username,
                    Name = model.Name,
                    Surname = model.Surname
                };

                IdentityResult identityResults = await _userManager.CreateAsync(user, model.Password);
                if (!identityResults.Succeeded)
                {
                    foreach (IdentityError error in identityResults.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                await _userManager.AddToRoleAsync(user, "User");

                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string? link = Url.Action("confirmEmail", "account", new { userId = user.Id, token }, HttpContext.Request.Scheme);

                _mailService.SendMail(user.Email, "Verify Email", $"Hi {user.UserName},\nClick the link below to verify your account:\n{link}");

                return RedirectToAction("index", "home");
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
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
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    ModelState.AddModelError("", "You don't have permission to access this page");
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
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            await _userManager.ConfirmEmailAsync(user!, token);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("index", "home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
                if (user is null || !await _userManager.IsInRoleAsync(user, "User"))
                {
                    user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                    if (user is null || !await _userManager.IsInRoleAsync(user, "User"))
                    {
                        ModelState.AddModelError("UsernameOrEmail", "User is not found");
                        return View(model);
                    }
                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                string? link = Url.Action("resetPassword", "account", new { userId = user.Id, token }, HttpContext.Request.Scheme);
                _mailService.SendMail(user.Email!, "Reset Password", $"Hi {user.UserName},\nClick the link below to reset your password:\n{link}");
            }
            return View(model);
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            ResetPasswordModel model = new()
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByIdAsync(model.UserId);
                if (user is null)
                {
                    return NotFound();
                }
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (!identityResult.Succeeded)
                {
                    foreach (IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return RedirectToAction("login", "account");
            }
            return View(model);
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Info()
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            return View(user);
        }
        public async Task<IActionResult> Update()
        {
            AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name!);
            UpdateProfileModel model = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProfileModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (await _userManager.CheckPasswordAsync(user!, model.Password))
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("Password", "Incorrect password");
                return View(model);
            }
            return View(model);
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
        //    await _userManager.AddToRolesAsync(user, ["SuperAdmin","Admin"]);
        //    return Json(user);
        //}
    }
}

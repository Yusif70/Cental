using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class ContactController : Controller
    {
        private readonly IRepository<Message> _repository;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(IRepository<Message> repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (await _userManager.IsInRoleAsync(user, "User"))
                    {
                        message.AppUserId = user.Id;
                    }
                    else
                    {
                        return Json(new { statusCode = 404, message = "You have to login to send a message" });
                    }
                }
                catch
                {
                    return Json(new { statusCode = 404, message = "You have to login to send a message" });
                }
                message.CreatedAt = DateTime.Now;
                await _repository.AddAsync(message);
                await _repository.SaveAsync();
                return Json(new { statusCode = 201, message = "Message sent successfully!" });
            }
            return Json(new { statusCode = 400, message = "Please fill in all the inputs!" });
        }
    }
}

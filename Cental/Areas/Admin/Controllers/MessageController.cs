using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class MessageController : Controller
    {
        private readonly IRepository<Message> _repository;
        public MessageController(IRepository<Message> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            List<Message> messages = await _repository.GetAll().Include(m => m.AppUser).ToListAsync();
            return View(messages);
        }
        [HttpDelete("/admin/message/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.RemoveAsync(id);
                await _repository.SaveAsync();
                return Json(new { statusCode = 200, message = "Message is deleted successfully!" });
            }
            catch
            {
                return Json(new { statusCode = 404, message = "Message is not found" });
            }
        }
    }
}

using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cental.Controllers
{
    public class CommentController(IRepository<Comment> repository, UserManager<AppUser> userManager) : Controller
    {
        private readonly IRepository<Comment> _repository = repository;
        private readonly UserManager<AppUser> _userManager = userManager;
        public async Task<IActionResult> Post([FromBody] Comment comment)
        {
            try
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    comment.AppUserId = user.Id;
                    comment.CreatedAt = DateTime.Now;
                    await _repository.AddAsync(comment);
                    await _repository.SaveAsync();
                    return Json(new { statusCode = 201, message = "Comment added succesfully!" });
                }
                return Json(new { statusCode = 404, message = "You have to login to send a message" });
            }
            catch
            {
                return Json(new { statusCode = 404, message = "You have to login to send a message" });
            }
        }
        [HttpPut("/home/comment/edit/{id}")]
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            Comment updatedComment = await _repository.GetAsync(id);
            if (ModelState.IsValid)
            {
                updatedComment.Message = comment.Message;
                updatedComment.LastUpdatedAt = DateTime.Now;
                _repository.Update(updatedComment);
                await _repository.SaveAsync();
                return Json(new { statusCode = 200, message = "Comment updated successfully!" });
            }
            return Json(new { statusCode = 400, message = "Please fill in all the inputs" });
        }
        public async Task<IActionResult> Reply([FromBody] Comment comment)
        {
            try
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    comment.AppUserId = user.Id;
                    comment.CreatedAt = DateTime.Now;
                    await _repository.AddAsync(comment);
                    await _repository.SaveAsync();
                    return Json(new { statusCode = 201, message = "Reply added successfully!" });
                }
                return Json(new { statusCode = 404, message = "You have to login to add a reply" });
            }
            catch
            {
                return Json(new { statusCode = 404, message = "You have to login to add a reply" });
            }
        }
    }
}

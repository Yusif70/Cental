using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _repository;
        public CategoryController(IRepository<Category> repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _repository.GetAll().Include(c => c.Blogs).ToListAsync();
            return View(categories);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    category.CreatedAt = DateTime.Now;
                    await _repository.AddAsync(category);
                    await _repository.SaveAsync();
                    return RedirectToAction("index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Name", "There is already one category with the same name");
                }
            }
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            Category category = await _repository.GetAsync(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            Category updatedCategory = await _repository.GetAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    updatedCategory.Name = category.Name;
                    updatedCategory.LastUpdatedAt = DateTime.Now;
                    _repository.Update(updatedCategory);
                    await _repository.SaveAsync();
                    return RedirectToAction("index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Name", "There is already one category with the same name");
                }
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoveAsync(id);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}

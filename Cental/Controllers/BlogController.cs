using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<Blog> _repository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Category> _categoryRepository;
        public BlogController(IRepository<Blog> repository, IRepository<Tag> tagRepository, IRepository<Category> categoryRepository)
        {
            _repository = repository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _repository.GetAll().Include(b => b.Author).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Blog blog = await _repository.GetAll().Include(b => b.Author).Include(b => b.BlogsTags).FirstAsync(b => b.Id == id);
            ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
            ViewBag.Categories = await _categoryRepository.GetAll().Include(c => c.Blogs).ToListAsync();
            ViewBag.Blogs = await _repository.GetAll().Include(b => b.Author).Where(b => b.Id != id).OrderByDescending(b => b.CreatedAt).ToListAsync();
            return View(blog);
        }
    }
}

using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class BlogController : Controller
    {
        private readonly IRepository<Blog> _repository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<BlogsTags> _blogsTagsRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(IRepository<Blog> repository, IRepository<Author> authorRepository, IRepository<Category> categoryRepository, IRepository<Tag> tagRepository, IRepository<Blog> blogRepository, IRepository<BlogsTags> blogsTagsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _blogRepository = blogRepository;
            _blogsTagsRepository = blogsTagsRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _repository.GetAll().Include(b => b.Category).Include(b => b.Author).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Add()
        {
            ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
            ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
            ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Blog blog)
        {
            string fileName = Guid.NewGuid().ToString() + blog.File?.FileName;
            string path = _webHostEnvironment.WebRootPath + "/admin/images/blogs/" + fileName;
            if (ModelState.IsValid)
            {
                blog.CreatedAt = DateTime.Now;
                blog.Image = "defaultBg.png";
                if (blog.File != null)
                {
                    blog.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await blog.File.CopyToAsync(fileStream);
                }
                await _repository.AddAsync(blog);
                await _repository.SaveAsync();
                foreach (int tagId in blog.TagIds)
                {
                    await _blogsTagsRepository.AddAsync(new BlogsTags()
                    {
                        BlogId = blog.Id,
                        TagId = tagId,
                    });
                }
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
            ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
            ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
            ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
            ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
            Blog blog = await _repository.GetAsync(id);
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            Blog updatedBlog = await _repository.GetAsync(id);
            string basePath = _webHostEnvironment.WebRootPath + "/admin/images/blogs/" + updatedBlog.Image;
            if (ModelState.IsValid)
            {
                if (updatedBlog.Image != "defaultBg.png")
                {
                    System.IO.File.Delete(basePath);
                }
                if (blog.File != null)
                {
                    string fileName = Guid.NewGuid().ToString() + blog.File?.FileName;
                    string path = _webHostEnvironment.WebRootPath + "/admin/images/blogs/" + fileName;
                    updatedBlog.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await blog.File!.CopyToAsync(fileStream);
                }
                updatedBlog.Title1 = blog.Title1;
                updatedBlog.Title2 = blog.Title2;
                updatedBlog.Description1 = blog.Description1;
                updatedBlog.Description2 = blog.Description2;
                updatedBlog.AuthorId = blog.AuthorId;
                updatedBlog.CategoryId = blog.CategoryId;
                updatedBlog.TagIds = blog.TagIds;
                updatedBlog.LastUpdatedAt = DateTime.Now;
                _repository.Update(blog);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
            ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
            ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _repository.GetAsync(id);
            if (blog.Image != "defaultBg.png")
            {
                string path = _webHostEnvironment.WebRootPath + "/admin/images/blogs/" + blog.Image;
                System.IO.File.Delete(path);
            }
            _repository.Remove(blog);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}

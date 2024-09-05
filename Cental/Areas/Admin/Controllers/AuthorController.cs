using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cental.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class AuthorController : Controller
    {
        private readonly IRepository<Author> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthorController(IRepository<Author> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _repository.GetAll().ToListAsync();
            return View(authors);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Author author)
        {
            string fileName = Guid.NewGuid().ToString() + author.File?.FileName;
            string path = _webHostEnvironment.WebRootPath + "/admin/images/authors/" + fileName;
            if (ModelState.IsValid)
            {
                author.CreatedAt = DateTime.Now;
                author.Image = "defaultPfp.png";
                if (author.File != null)
                {
                    author.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await author.File.CopyToAsync(fileStream);
                }
                await _repository.AddAsync(author);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            Author author = await _repository.GetAsync(id);
            return View(author);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Author author)
        {
            Author updatedAuthor = await _repository.GetAsync(id);
            string basePath = _webHostEnvironment.WebRootPath + "/admin/images/authors/" + updatedAuthor.Image;
            if (ModelState.IsValid)
            {
                if (updatedAuthor.Image != "defaultPfp.png")
                {
                    System.IO.File.Delete(basePath);
                }
                if (author.File != null)
                {
                    string fileName = Guid.NewGuid().ToString() + author.File.FileName;
                    string path = _webHostEnvironment.WebRootPath + "/admin/images/authors/" + fileName;
                    updatedAuthor.Image = fileName;
                    using FileStream fileStream = System.IO.File.Open(path, FileMode.Create);
                    await author.File.CopyToAsync(fileStream);
                }
                updatedAuthor.Name = author.Name;
                updatedAuthor.Surname = author.Surname;
                updatedAuthor.Description = author.Description;
                updatedAuthor.LastUpdatedAt = DateTime.Now;
                _repository.Update(updatedAuthor);
                await _repository.SaveAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            Author author = await _repository.GetAsync(id);
            if (author.Image != "defaultPfp.png")
            {
                string path = _webHostEnvironment.WebRootPath + "/admin/images/authors/" + author.Image;
                System.IO.File.Delete(path);
            }
            _repository.Remove(author);
            await _repository.SaveAsync();
            return RedirectToAction("index");
        }
    }
}
